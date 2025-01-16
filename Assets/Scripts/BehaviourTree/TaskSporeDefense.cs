using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSporeDefense : Node
{
    private Unit _unit;
    private HealthComponent _healthComponent;
    private bool _isMoving;

    public TaskSporeDefense(Unit unit, HealthComponent healthComponent, bool isMoving)
    {
        _unit = unit;
        _healthComponent = healthComponent;
        _isMoving = isMoving;
    }

    public override NodeState Evaluate()
    {

        if (_isMoving)
        {
            Debug.Log(_isMoving);
            _healthComponent.dmgMultiplier = 0.5f;
        }
        else 
        {
            Debug.Log(_isMoving);
            _healthComponent.dmgMultiplier = 1f;
        }       

        return NodeState.SUCCESS;
    }
}