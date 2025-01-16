using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class TaskGoTo : Node
{
    private Transform _tranform;
    private Transform _positionToGo;

    public TaskGoTo(Transform transform, Transform positionToGO)
    {
        _tranform = transform;
        _positionToGo = positionToGO;
    }

    public override NodeState Evaluate()
    {
        Vector3 movement = _positionToGo.position - _tranform.position;
        movement = movement.normalized * TestBehaviourTree.speed;
        _tranform.position += movement * Time.deltaTime;

        state = NodeState.RUNNING;
        return state;
    }
}
