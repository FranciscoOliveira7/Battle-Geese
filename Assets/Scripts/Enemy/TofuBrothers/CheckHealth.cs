using BehaviourTree;
using UnityEngine;

public class TaskCheckHealth : Node
{
    private EnemyHealthComponent _tofuBrotherHp1;
    private EnemyHealthComponent _tofuBrotherHp2;

    public TaskCheckHealth(EnemyHealthComponent tofuBrotherHp1, EnemyHealthComponent tofuBrotherHp2)
    {
        _tofuBrotherHp1 = tofuBrotherHp1;
        _tofuBrotherHp2 = tofuBrotherHp2;
    }

    public override NodeState Evaluate()
    {

        if (_tofuBrotherHp1.Health <= 0.5f * _tofuBrotherHp1.MaxHealth && _tofuBrotherHp2.Health <= 0.5f * _tofuBrotherHp2.MaxHealth)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
