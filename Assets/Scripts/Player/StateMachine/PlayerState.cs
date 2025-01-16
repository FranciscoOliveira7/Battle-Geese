using UnityEngine.InputSystem;

public abstract class PlayerState : IState
{
    protected PlayerStateMachine stateMachine;

    protected Player player;
    protected ReusableData reusableData;

    public PlayerState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        player = stateMachine.Player;
        reusableData = stateMachine.ReusableData;
    }

    public virtual void Enter()
    {
        AddInputCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputCallbacks();
    }

    public virtual void HandleInput() { }

    public virtual void Update() { }

    public virtual void PhysicsUpdate() { }

    protected virtual void AddInputCallbacks() { }

    protected virtual void RemoveInputCallbacks() { }

    protected void SwitchWeapon(InputAction.CallbackContext context) => player.SwitchWeapon(context);

    protected void OnAttack(InputAction.CallbackContext context)
    {
        if (player.Weapons[player.EquipedWeapon].Data == null) return;
        stateMachine.ChangeState(stateMachine.AttackingState);
    }
}

