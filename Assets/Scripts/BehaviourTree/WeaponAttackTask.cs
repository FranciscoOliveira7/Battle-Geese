using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;


public class WeaponAttackTask : Node
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    private Transform _target;
    private Weapon _weapon;
    private Unit _unit;
    private AnimationEventHandler _eventHandler;
    private Animator _animator;
    private EnemySpriteFlip _flip;

    public WeaponAttackTask(Unit unit, Weapon weapon, Transform target, Animator animator)
    {
        _target = target;
        _weapon = weapon;
        _unit = unit;
        _animator = animator;
        _flip = unit.GetComponent<EnemySpriteFlip>();
    }

    public override NodeState Evaluate()
    {
        // if (_weapon.isAttacking) return NodeState.FAILURE;
        if (_weapon.isAttacking) return NodeState.SUCCESS;
        if (_target == null)
        {
            Debug.LogError("WeaponAttackTask: _target is null.");
            state = NodeState.FAILURE;
            return state;
        }

        Vector3 direction = _target.transform.position - _unit.transform.position;

        _animator.SetTrigger(Attack);
        _weapon.Direction = direction;
        _flip.ManuallyFlip(direction.x > 0);
        // _unit.transform.localScale = new Vector3(direction.x < 0 ? 1 : -1, 1, 1);
        _weapon.Enter();

        state = NodeState.SUCCESS;
        return state;
    }

}
