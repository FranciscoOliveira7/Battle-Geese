using System.Collections.Generic;

namespace BehaviourTree
{
    public class InterruptionNode : Node
    {
        private Node _conditionNode;
        private Node _interruptionNode;
        private Node _mainNode;

        public InterruptionNode(Node conditionNode, Node interruptionNode, Node mainNode)
        {
            _conditionNode = conditionNode;
            _interruptionNode = interruptionNode;
            _mainNode = mainNode;
        }

        public override NodeState Evaluate()
        {
            if (_conditionNode.Evaluate() == NodeState.SUCCESS)
            {
                state = _interruptionNode.Evaluate();
            }
            else
            {
                state = _mainNode.Evaluate();
            }

            return state;
        }
    }
}