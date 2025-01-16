using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using BehaviourTree;
using UnityEngine;
using UnityEngine.UIElements;


public class SporeThrowTask : Node
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    private Transform _target;
    private Weapon _weapon;
    private Unit _unit;
    private AnimationEventHandler _eventHandler;
    private Animator _animator;
    private EnemySpriteFlip _flip;

    public Sporehit sporehit;
    public GameObject goonpos;

    public SporeThrowTask(Unit unit, Weapon weapon, Animator animator)
    {
        _weapon = weapon;
        _unit = unit;
        _animator = animator;
        _flip = unit.GetComponent<EnemySpriteFlip>();
    }

    public override NodeState Evaluate()
    {

        goonpos = GameObject.Find("SporetacusRoom/Goonspos");
        sporehit = goonpos.GetComponent<Sporehit>();

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
