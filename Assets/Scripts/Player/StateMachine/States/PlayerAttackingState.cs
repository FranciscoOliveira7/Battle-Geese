using System;
using UnityEngine;

public class PlayerAttackingState : PlayerMovementState
{
    private Weapon _currentWeapon;
    private Camera _camera;

    private Plane _plane = new Plane(Vector3.down, 0.35f);
    private bool _attackPerformed;

    private bool _hasReleasedKey;

    public PlayerAttackingState(PlayerStateMachine player) : base(player) => _camera = Camera.main;

    public override void Enter()
    {
        base.Enter();

        reusableData.SpeedMultiplier = player.Data.AttackingMovingSpeedMultiplier;

        _currentWeapon = player.Weapons[player.EquipedWeapon];
        _currentWeapon.OnExit += ExitHandler;

        // Make player look to where the cursor is on the screen
        UpdateAim();

        _attackPerformed = false;
        _currentWeapon.AnimationSpeedMultiplier = player.AttackSpeed;
        _currentWeapon.Enter();
    }

    public override void Update()
    {
        base.Update();
        
        player.Animator.SetBool("running", reusableData.MovementInput != Vector2.zero);

        _currentWeapon.CurrentInput = player.Input.PlayerActions.Attack.ReadValue<float>() > 0.5f;
        if (!_hasReleasedKey && _currentWeapon.CurrentInput == false) _hasReleasedKey = true;

        if (!_hasReleasedKey) UpdateAim();

        if (_attackPerformed) stateMachine.ChangeState(stateMachine.IdlingState);
    }

    private void UpdateAim()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (_plane.Raycast(ray, out float distance))
        {
            _currentWeapon.Direction = ray.GetPoint(distance) - player.transform.position;

            player.FlipSprite(ray.GetPoint(distance).x - player.transform.position.x < 0);
        }
    }

    private void ExitHandler() => _attackPerformed = true;

    public override void Exit()
    {
        base.Exit();
        
        _hasReleasedKey = false;
        
        _currentWeapon.Exit();
    }
}
