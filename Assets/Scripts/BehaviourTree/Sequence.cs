using System;
using System.Collections.Generic;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        private int index;
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            for (int i = index; i < children.Count; i++)
            {
                switch (children[i].Evaluate())
                {
                    case NodeState.FAILURE:
                        index = 0;
                        return NodeState.FAILURE;
                    
                    case NodeState.SUCCESS: continue;
                    
                    case NodeState.RUNNING:
                        index = i;
                        return NodeState.RUNNING;
                    
                    default: throw new ArgumentOutOfRangeException();
                }
            }

            // foreach (Node node in children)
            // {
            //     switch (node.Evaluate())
            //     {
            //         case NodeState.FAILURE:
            //             state = NodeState.FAILURE;
            //             return state;
            //         case NodeState.SUCCESS:
            //             continue;
            //         case NodeState.RUNNING:
            //             anyChildIsRunning = true;
            //             continue;
            //         default:
            //             state = NodeState.SUCCESS;
            //             return state;
            //     }
            // }

            index = 0;
            return NodeState.SUCCESS;
        }

    }
}