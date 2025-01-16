using BehaviourTree;
using Unity.VisualScripting;
using UnityEngine;

public class CheckTargetInSight : Node
{
    private Ray _ray;
    private Unit _unit;

    public CheckTargetInSight(Unit unit) => _unit = unit;

    public override NodeState Evaluate()
    {
        _ray.origin = _unit.transform.position;
        _ray.direction = _unit.movingTarget.position - _unit.transform.position;
        if (Physics.Raycast(_ray, (_unit.movingTarget.position - _unit.transform.position).magnitude, 1 << 10))
        {
            return NodeState.FAILURE;
        }
        
        return NodeState.SUCCESS;
    }
}
