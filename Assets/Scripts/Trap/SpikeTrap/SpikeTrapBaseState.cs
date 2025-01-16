public abstract class SpikeTrapBaseState : IState
{
    protected SpikeTrapStateMachine StateMachine;

    protected SpikeTrapBaseState(SpikeTrapStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void HandleInput();
    public abstract void PhysicsUpdate();
    public abstract void Update();
}
