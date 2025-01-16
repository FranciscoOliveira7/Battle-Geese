using BehaviourTree;
using System.Collections;
using UnityEngine;

public class TaskAvoid : Node
{
    private Unit _unit;
    private Transform _avoidant;
    private DashAttackTask _dashTask;
    private WeaponAttackTask _weaponAttackTask;

    private Transform _player;

    public TaskAvoid(Unit unit, Transform avoidant, Transform player)
    {
        _unit = unit;
        _avoidant = avoidant;
        _player = player;
    }



    public override NodeState Evaluate()
    {



        state = NodeState.RUNNING;
        return state;
    }


}
