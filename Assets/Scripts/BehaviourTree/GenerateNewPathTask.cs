using System;
using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class GenerateNewPathTask : Node
{
    private Unit _unit;
    private Animator _animator;

    public GenerateNewPathTask(Unit unit, Animator animator)
    {
        _unit = unit;
        _animator = animator;
    }

    public override NodeState Evaluate()
    {
        if (_unit.targetFunc != null)
            _unit.UpdateCustomPosition();
        
        _animator.SetBool("isWalking", true);
        _unit.isStopped = false;
        
        state = NodeState.SUCCESS;
        return state;
    }
}