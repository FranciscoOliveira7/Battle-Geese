using UnityEngine;

public class PlayerDashingState : PlayerMovementState
{
    private Vector3 _dashDirection;
    private float _timeElapsed;

    public PlayerDashingState(PlayerStateMachine player) : base(player) { }

    public override void Enter()
    {
        player.IsDashing = true;
        
        player.Animator.SetBool("dashing", true);
        
        _dashDirection = GetMovementInputDirection();

        _timeElapsed = 0f;

        reusableData.SpeedMultiplier = 1f;

        player.RigidBody.AddForce(player.Data.BaseSpeed * player.Data.DashSpeedMultiplier * _dashDirection, ForceMode.VelocityChange);
    }

    public override void HandleInput() { }

    public override void Exit()
    {
        player.IsDashing = false;
        player.Animator.SetBool("dashing", false);
        // Start dash cooldown for the player to not spam
        player.Input.DisableAction(player.Input.PlayerActions.Dash, player.Data.DashCooldown);
    }

    public override void Update()
    {
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed >= player.Data.DashDuration) stateMachine.ChangeState(stateMachine.RunningState);
    }

    public override void PhysicsUpdate()
    {
        Decelerate();
    }
}
