using UnityEngine;

namespace BehaviourTree
{
    public enum CheckType
    {
        inside,
        outside,
    }
    public class TaskCheckDistance : Node
    {
        private Unit _unit;
        private float _distanceThreshold;
        private CheckType _checkType;
        private Transform _target;

        public TaskCheckDistance(Unit unit, Transform target, float distanceThreshold, CheckType checkType = CheckType.inside)
        {
            _target = target;
            _unit = unit;
            _distanceThreshold = distanceThreshold;
            _checkType = checkType;
        }

        public override NodeState Evaluate()
        {
            float distanceToTarget = Vector3.Distance(_unit.transform.position, _target.position);
            
            if (_checkType == CheckType.inside)
            {
                return distanceToTarget > _distanceThreshold ? NodeState.FAILURE : NodeState.SUCCESS;
            }
            
            if (_distanceThreshold == 8 && distanceToTarget > _distanceThreshold) Debug.Log("Bro is far");
            return distanceToTarget > _distanceThreshold ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}
