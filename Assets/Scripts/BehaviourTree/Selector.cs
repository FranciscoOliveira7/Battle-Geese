using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Selector : Node
    {
        private int index;
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            for (int i = index; i < children.Count; i++)
            {
                switch (children[i].Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        index = 0;
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }
            
            // foreach (Node node in children)
            // {
            //     switch (node.Evaluate())
            //     {
            //         case NodeState.FAILURE:
            //             continue;
            //         case NodeState.SUCCESS:
            //             index = 0;
            //             state = NodeState.SUCCESS;
            //             return state;
            //         case NodeState.RUNNING:
            //             state = NodeState.RUNNING;
            //             return state;
            //         default:
            //             continue;
            //     }
            // }

            index = 0;
            state = NodeState.FAILURE;
            return state;
        }

    }

}