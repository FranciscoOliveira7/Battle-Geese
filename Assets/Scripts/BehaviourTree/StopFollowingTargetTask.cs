using System;
using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class StopFollowingTargetTask : Node
{
    private Unit _unit;
    private Animator _animator;
    private Weapon _weap;

    public StopFollowingTargetTask(Unit unit, Animator animator)
    {
        _unit = unit;
        _animator = animator;
    }
    
    public StopFollowingTargetTask(Unit unit, Animator animator, Weapon weap)
    {
        _unit = unit;
        _animator = animator;
        _weap = weap;
    }

    public override NodeState Evaluate()
    {
        _unit.isStopped = true;
        _unit.rb.velocity = Vector3.zero;
        
        state = NodeState.SUCCESS;
        return state;
    }
}