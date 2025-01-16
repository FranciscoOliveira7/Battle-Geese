using System;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;
using Tree = BehaviourTree.Tree;

public class BrocolliBT : Tree
{
    private Transform _target;
    private Weapon _weapon;
    private Unit _unit;
    private Animator _animator;
    public static float attackCooldown = 1f;

    protected override Node SetupTree()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _unit = GetComponent<Unit>();
        _target = MainManager.Instance.ClosestPlayer(transform.position).transform;
        _unit.movingTarget = _target;
        _weapon = transform.Find("Weapon").GetComponent<Weapon>();
        _weapon.Equip();
        EnemyHealthComponent healthComponent = GetComponent<EnemyHealthComponent>();
        _weapon.User = healthComponent;

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new Sequence(new List<Node>
                {                    
                    new WaitTask(attackCooldown),
                    new WeaponAttackTask(_unit, _weapon, _target, _animator),
                }),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new TaskCheckDistance(_unit, _target, 2f),
                        new StopFollowingTargetTask(_unit, _animator),
                    }),
                    new StartFollowingTargetTask(_target, _unit, _animator),
                }),
            })
        });
        
        root.SetData("target", GameObject.FindGameObjectWithTag("Player").transform);

        return root;
    }
}
