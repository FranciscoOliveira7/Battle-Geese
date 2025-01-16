using System.Collections;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public ReusableData ReusableData { get; }

    public Player Player { get; }

    // Player States
    public PlayerIdlingState IdlingState { get; }
    public PlayerRunningState RunningState { get; }
    public PlayerDashingState DashingState { get; }
    public PlayerAttackingState AttackingState { get; }
    public PlayerEnteringDoorState EnteringDoorState { get; }
    public PlayerDeadState DeadState { get; }
    public PlayerRevivingState RevivingState { get; }

    public PlayerStateMachine(Player player)
    {
        Player = player;

        ReusableData = new ReusableData();

        // Inicializa todos os Estados
        IdlingState = new PlayerIdlingState(this);
        RunningState = new PlayerRunningState(this);
        DashingState = new PlayerDashingState(this);
        AttackingState = new PlayerAttackingState(this);
        EnteringDoorState = new PlayerEnteringDoorState(this);
        DeadState = new PlayerDeadState(this);
        RevivingState = new PlayerRevivingState(this);
    }
}
