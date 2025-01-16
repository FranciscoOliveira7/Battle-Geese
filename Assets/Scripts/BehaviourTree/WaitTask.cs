using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class WaitTask : Node
{
    private float _duration, _timeElapsed;

    public WaitTask(float duration)
    {
        _duration = duration;
        _timeElapsed = 0f;
    }

    public override NodeState Evaluate()
    {
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed < _duration)
        {
            state = NodeState.RUNNING;
            return state;
        }
        
        _timeElapsed = 0f;
        
        state = NodeState.SUCCESS;
        return state;
    }
}