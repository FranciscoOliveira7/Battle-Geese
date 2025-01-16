using Photon.Pun;
using UnityEngine;

public class SpikeTrapActiveState : SpikeTrapBaseState
{
    public SpikeTrapActiveState(SpikeTrapStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        StateMachine.SpikeTrap.OnCollision += OnCollisionEnter;
        StateMachine.SpikeTrap.spriteRenderer.sprite = null; // No sprite when active
    }

    private void OnCollisionEnter(Collider other)
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(StateMachine.SpikeTrap.Damage, Vector3.up, null);
            }
        }

        StateMachine.SpikeTrap.PlayAudio();
        StateMachine.ChangeState(StateMachine.InactiveState);
    }



    public override void Exit()
    {
        StateMachine.SpikeTrap.OnCollision -= OnCollisionEnter;
    }

    public override void HandleInput() { }
    public override void PhysicsUpdate() { }
    public override void Update() { }
}
