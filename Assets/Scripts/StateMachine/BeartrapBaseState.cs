using System;

public abstract class BeartrapBaseState : IState
{
    protected BeartrapStateMachine stateMachine;

    public BeartrapBaseState(BeartrapStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void HandleInput();

    public abstract void Update();

    public abstract void PhysicsUpdate();
}