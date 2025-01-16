using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerRunningState : PlayerMovementState
{
    public PlayerRunningState(PlayerStateMachine player) : base(player) { }

    public override void Enter()
    {
        base.Enter();

        player.Animator.SetBool("running", true);

        reusableData.SpeedMultiplier = 1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.Animator.SetBool("running", false);
    }

    public override void Update()
    {
        base.Update();

        if (reusableData.MovementInput.x != 0)
        {
            player.FlipSprite(reusableData.MovementInput.x < 0);
            return;
        }

        if (reusableData.MovementInput.y == 0) stateMachine.ChangeState(stateMachine.IdlingState);
    }

    private void OnDash(InputAction.CallbackContext context) => stateMachine.ChangeState(stateMachine.DashingState);

    protected override void AddInputCallbacks()
    {
        player.Input.PlayerActions.Dash.performed += OnDash;
        player.Input.PlayerActions.SwitchWeapon.performed += SwitchWeapon;
        player.Input.PlayerActions.Attack.performed += OnAttack;
    }

    protected override void RemoveInputCallbacks()
    {
        player.Input.PlayerActions.Dash.performed -= OnDash;
        player.Input.PlayerActions.SwitchWeapon.performed -= SwitchWeapon;
        player.Input.PlayerActions.Attack.performed -= OnAttack;
    }
}
