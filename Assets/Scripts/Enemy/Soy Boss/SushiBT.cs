using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;
using Tree = BehaviourTree.Tree;

public class SushiBT : Tree
{
    private Transform _target;
    private Weapon _weapon;
    private Unit _unit;
    private Animator _animator;
    
    protected override Node SetupTree()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _unit = GetComponent<Unit>();
        _target = MainManager.Instance.ClosestPlayer(transform.position).transform;
        _unit.movingTarget = _target;
        _weapon = transform.Find("Weapon").GetComponent<Weapon>();
        _weapon.Equip();
        
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new TaskCheckDistance(_unit, _target, 0.5f),
                new StopFollowingTargetTask(_unit, _animator),
                new WeaponAttackTask(_unit, _weapon, _target, _animator),
                new WaitTask(0.8f),
            }),
            new StartFollowingTargetTask(_target, _unit, _animator),
        });

        return root;
    }
}
