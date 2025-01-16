using System;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;
using Tree = BehaviourTree.Tree;

public class AppleBT : Tree
{
    public float dashSpeed = 6f;
    public float attackRange = 5f;
    public float attackCooldown = 4f;
    private Animator _animator;
    Unit _unit;
    Transform _target;

    protected override Node SetupTree()
    {
        _unit = GetComponent<Unit>();
        _target = MainManager.Instance.ClosestPlayer(transform.position).transform;
        _unit.movingTarget = _target;
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        EnemyHealthComponent healthComponent = GetComponent<EnemyHealthComponent>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new WaitTask(attackCooldown),
                new Sequence(new List<Node>{
                    new TaskCheckDistance(_unit, _target, attackRange, CheckType.inside),
                    new CheckTargetInSight(_unit),
                    new TaskCheckDistance(_unit, _target, 1.5f, CheckType.outside),
                    new DashAttackTask(_unit, 0.5f, 1f, dashSpeed, _animator),
                }),
            })
        });
        
        return root;
    }
}