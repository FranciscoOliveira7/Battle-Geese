using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using BehaviourTree;
using UnityEngine;
using UnityEngine.UIElements;


public class CarotThrowTask : Node
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    private Transform _target;
    private Weapon _weapon;
    private Unit _unit;
    private AnimationEventHandler _eventHandler;
    private Animator _animator;
    private EnemySpriteFlip _flip;
    private Transform _spearspot;

    public Sporehit sporehit;
    public GameObject goonpos;

    public CarotThrowTask(Unit unit, Weapon weapon, Animator animator, Transform target)
    {
        _weapon = weapon;
        _unit = unit;
        _animator = animator;
        _flip = unit.GetComponent<EnemySpriteFlip>();
        // _spearspot = spearspot;
    }

    public override NodeState Evaluate()
    {

        sporehit = _spearspot.GetComponent<Sporehit>();

        _target = sporehit._throwSpot;

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
