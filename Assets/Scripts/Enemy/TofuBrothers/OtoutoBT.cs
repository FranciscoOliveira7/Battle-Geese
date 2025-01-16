using System;
using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;
using Tree = BehaviourTree.Tree;

public class OtoutoBt : Tree
{
    private Transform _target;
    private Weapon _weapon1;
    private Weapon _weapon2;
    public Unit _unit;
    private Animator _animator;
    public bool hasDashes = true;
    public bool isAgresive;
    private Transform _avoidant;
    [SerializeField] private LayerMask wallLayerMask; // Layer mask to detect walls
    public float attackRange = 3f;

    protected override Node SetupTree()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _unit = GetComponent<Unit>();
        _target = MainManager.Instance.ClosestPlayer(transform.position).transform;
        _avoidant = GameObject.FindGameObjectWithTag("Avoidant").transform;
        _unit.movingTarget = _target;
        _weapon1 = transform.Find("PrimaryWeapon").GetComponent<Weapon>();
        _weapon2 = transform.Find("SecondaryWeapon").GetComponent<Weapon>();
        _weapon1.Equip();
        _weapon2.Equip();

        EnemyHealthComponent healthComponent = GetComponent<EnemyHealthComponent>();

        DashAttackTask dashAttackTask = new DashAttackTask(_unit, 0.7f, 1f, 10, _animator);
        dashAttackTask.OnDashCompleted += () => hasDashes = false; // Subscribe to the event

        Node root = new Selector(new List<Node>
        {


            new Sequence(new List<Node>
            {
                new ConditionalNode(() => hasDashes),


                dashAttackTask, // Use the created DashAttackTask instance
            }),

            new Sequence(new List<Node>
            {
                // Check is aggressive
                new ConditionalNode(() => isAgresive),

                new Sequence(new List<Node>
                {
                    // Follow target
                    new StartFollowingTargetTask(_target, _unit, _animator, isAgresive),

                    new TaskCheckDistance(_unit, _target, attackRange, CheckType.inside),
                    //new CheckTargetInSight(_unit),
                    //new TaskCheckDistance(_unit, 1.5f, CheckType.outside),
                    new WeaponAttackTask(_unit, _weapon1, _target, _animator),

                    new WaitTask(1f),
                }),
            }),

            // Avoid
            new Sequence(new List<Node>
            {
                new ConditionalNode(() => !isAgresive),

                new Sequence(new List<Node>
                {
                    new StartFollowingTargetTask(_avoidant, _unit, _animator, isAgresive),
                 

                    // Target in this case is the brother position for the TaskUpdateAvoidantPosition
                    new TaskUpdateAvoidantPosition(_avoidant, wallLayerMask, _unit.transform),

                    new WeaponAttackTask(_unit, _weapon2, _target, _animator),

                    new WaitTask(1f),
                }),
            }),
        });

        root.SetData("target", MainManager.Instance.ClosestPlayer(transform.position).transform);

        return root;
    }
}







