using UnityEngine;

public class PlayerMovementState : PlayerState
{
    public PlayerMovementState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void HandleInput() => ReadMovementInput();

    public override void PhysicsUpdate() => Move();

    private void Move()
    {
        // Player acceleration
        float xMovement = Mathf.Lerp(player.RigidBody.velocity.x, player.Data.BaseSpeed * reusableData.SpeedMultiplier * reusableData.MovementInput.x, Time.deltaTime * player.Data.MoveAcceleration);
        float zMovement = Mathf.Lerp(player.RigidBody.velocity.z, player.Data.BaseSpeed * reusableData.SpeedMultiplier * reusableData.MovementInput.y, Time.deltaTime * player.Data.MoveAcceleration);

        Vector3 movement = new(xMovement - player.RigidBody.velocity.x, 0f, zMovement - player.RigidBody.velocity.z);
        
        player.RigidBody.AddForce(movement, ForceMode.VelocityChange);
    }

    private void ReadMovementInput()
    {
        reusableData.MovementInput = player.Input.PlayerActions.Move.ReadValue<Vector2>();
    }

    // Returns the direction in 3D space of the player
    protected Vector3 GetMovementInputDirection() => new(reusableData.MovementInput.x, 0f, reusableData.MovementInput.y);

    protected void Decelerate()
    {
        if (player.RigidBody.velocity.magnitude > 0.1f)
        {
            player.RigidBody.AddForce(-player.RigidBody.velocity * 3, ForceMode.Acceleration);
        }
    }
}