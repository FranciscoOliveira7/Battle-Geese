using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdlingState : PlayerMovementState
{
    public PlayerIdlingState(PlayerStateMachine player) : base(player) { }

    public override void Enter()
    {
        base.Enter();

        reusableData.SpeedMultiplier = 0f;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (reusableData.MovementInput != Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.RunningState);
        }
    }

    protected override void AddInputCallbacks()
    {
        player.Input.PlayerActions.SwitchWeapon.performed += SwitchWeapon;
        player.Input.PlayerActions.Attack.performed += OnAttack;
    }

    protected override void RemoveInputCallbacks()
    {
        player.Input.PlayerActions.SwitchWeapon.performed -= SwitchWeapon;
        player.Input.PlayerActions.Attack.performed -= OnAttack;
    }
}
