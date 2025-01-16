using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BeartrapActiveState : BeartrapBaseState
{
    public BeartrapActiveState(BeartrapStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {

        stateMachine.Beartrap.OnCollision += OnCollisionEnter;
        stateMachine.Beartrap.spriteRenderer.sprite = stateMachine.Beartrap.sprites[0];
    }


    private void OnCollisionEnter(Collider other)
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(stateMachine.Beartrap.Damage, Vector3.up, null);
            }
        }
        
        stateMachine.ChangeState(stateMachine.InactiveState);
    }
    public override void Exit()
    {
        stateMachine.Beartrap.OnCollision -= OnCollisionEnter;
    }

    public override void HandleInput()
    {
    }

    public override void PhysicsUpdate()
    {
    }

    public override void Update()
    {

    }
}
