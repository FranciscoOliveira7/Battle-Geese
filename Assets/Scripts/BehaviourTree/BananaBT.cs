using System;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Tree = BehaviourTree.Tree;

public class BananaBT : Tree
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

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    // new StopFollowingTargetTask(_unit, _animator),
                    new TaskCheckDistance(_unit, _target, 7),
                    new WeaponAttackTask(_unit, _weapon, _target, _animator),
                    // new StopFollowingTargetTask(_unit, _animator, _weapon),
                    // new GenerateNewPathTask(_unit, _animator),
                    new WaitTask(1f),
                }),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new TaskCheckDistance(_unit, _target, 8, CheckType.outside),
                        new StartFollowingTargetTask(_target, _unit, _animator),
                    }),
                    new Sequence(new List<Node>
                    {
                        new TaskCheckDistance(_unit, _target, 3),
                        new StartFollowingTargetTask(RunAwayFromTarget, _unit, _animator),
                    }),
                    new Sequence(new List<Node>
                    {
                        new StartFollowingTargetTask(HorseAround, _unit, _animator),
                    }),
                })
            })
        });
        
        root.SetData("target", GameObject.FindGameObjectWithTag("Player").transform);

        return root;
    }
    
    public Vector3 HorseAround()
    {
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        Vector3 direction = new Vector3(x, 0, z).normalized;

        Vector3 newPosition = transform.position + direction * 2f;

        return newPosition;
        // and supposedly it will magically work and go to the random circle around the enemy
    }
    
    public Vector3 RunAwayFromTarget()
    {
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        Vector3 targetDirection = (_target.position - transform.position).normalized;
        
        Vector3 direction = new Vector3(x, 0, z).normalized;

        Vector3 newPosition = transform.position + 2 * (direction - targetDirection);

        return newPosition;
        // and supposedly it will magically work and go to the random circle around the enemy
    }
}
