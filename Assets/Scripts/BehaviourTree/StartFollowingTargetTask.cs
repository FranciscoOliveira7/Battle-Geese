using System;
using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class StartFollowingTargetTask : Node
{
    private Transform _target;
    private Func<Vector3> _customTarget;
    private Unit _unit;
    private Animator _animator;
    private bool _isAgresive;
    private bool isUsingCustomFunction;

    public StartFollowingTargetTask(Transform target, Unit unit, Animator animator, bool isAgresive)
    {
        isUsingCustomFunction = false;
        _target = target;
        _unit = unit;
        _animator = animator;
        _isAgresive = isAgresive;
    }
    
    public StartFollowingTargetTask(Transform target, Unit unit, Animator animator)
    {
        isUsingCustomFunction = false;
        _target = target;
        _unit = unit;
        _animator = animator;
        _isAgresive = true;
    }
    
    public StartFollowingTargetTask(Func<Vector3> target, Unit unit, Animator animator)
    {
        isUsingCustomFunction = true;
        _unit = unit;
        _customTarget = target;
        _animator = animator;
        _isAgresive = true;
    }

    public override NodeState Evaluate()
    {
        //if (_isAgresive == true)
        //{
        //    _animator.SetBool("isWalking", true);
        //    _animator.SetBool("isRetreating", false);
        //}
        //if (_isAgresive == false)
        //{
        //    _animator.SetBool("isRetreating", true);
        //    _animator.SetBool("isWalking", false);
        //}
        _animator.SetBool("isWalking", true);
        if (isUsingCustomFunction)
        {
            if (_unit.targetFunc != _customTarget || _unit.targetType != TargetType.custom)
                _unit.SetCustomPosition(_customTarget);
        }
        else
        {
            _unit.SetTarget(_target);
        }
        _unit.isStopped = false;
        
        state = NodeState.SUCCESS;
        return state;
    }
}