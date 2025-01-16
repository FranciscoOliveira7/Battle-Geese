using BehaviourTree;
using System;

public class ActionNode : Node
{
    private Action _action;

    public ActionNode(Action action)
    {
        _action = action;
    }

    public override NodeState Evaluate()
    {
        _action.Invoke();
        return NodeState.SUCCESS;
    }
}






