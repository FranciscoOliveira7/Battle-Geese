using System;

namespace BehaviourTree
{
    public class ConditionalNode : Node
    {
        private Func<bool> _condition;

        public ConditionalNode(Func<bool> condition)
        {
            _condition = condition;
        }

        public override NodeState Evaluate()
        {
            if (_condition())
            {
                state = NodeState.SUCCESS;
            }
            else
            {
                state = NodeState.FAILURE;
            }

            return state;
        }
    }
}
