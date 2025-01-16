using UnityEngine;

public class SpikeTrapInactiveState : SpikeTrapBaseState
{
    private float elapsedTime;

    public SpikeTrapInactiveState(SpikeTrapStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        StateMachine.SpikeTrap.spriteRenderer.sprite = StateMachine.SpikeTrap.inactiveSprite;
        elapsedTime = 0f;
    }

    public override void Exit() { }

    public override void HandleInput() { }

    public override void PhysicsUpdate() { }

    public override void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= StateMachine.SpikeTrap.CooldownTime)
        {
            StateMachine.ChangeState(StateMachine.ActiveState);
        }
    }
}
