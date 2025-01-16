using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class WaitTaskBernas : Node
{
    private float _duration, _timeElapsed;

    public WaitTaskBernas(float duration)
    {
        _duration = duration;
        _timeElapsed = 0f;
    }

    public override NodeState Evaluate()
    {
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed < _duration)
        {
            state = NodeState.FAILURE;
            return state;
        }
        
        _timeElapsed = 0f;
        
        state = NodeState.SUCCESS;
        return state;
    }
}