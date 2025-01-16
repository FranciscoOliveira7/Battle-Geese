using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeartrapInactiveState : BeartrapBaseState
{

    float elapsedTime;
    public BeartrapInactiveState(BeartrapStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        elapsedTime = 0;
        stateMachine.Beartrap.spriteRenderer.sprite = stateMachine.Beartrap.sprites[1];
    }


    public override void Exit()
    {
    }

    public override void HandleInput()
    {
    }

    public override void PhysicsUpdate()
    {
    }

    public override void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= stateMachine.Beartrap.CooldowndTime)
        {
            stateMachine.ChangeState(stateMachine.ActiveState);
        }
    }
}
