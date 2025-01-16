using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;
using Tree = BehaviourTree.Tree;

public class SporetacusBT : Tree
{
    private Transform _target;
    private Weapon _weapon;
    private Weapon _weapon2;
    private Unit _unit;
    private Animator _animator;
    public static float attackCooldown = 1f;
    public static float spawnCooldown = 5f;

    protected override Node SetupTree()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _unit = GetComponent<Unit>();
        _target = MainManager.Instance.ClosestPlayer(transform.position).transform;
        _unit.movingTarget = _target;
        _weapon = transform.Find("Weapon").GetComponent<Weapon>();
        _weapon2 = transform.Find("SecondaryWeapon").GetComponent<Weapon>();
        _weapon.Equip();
        EnemyHealthComponent healthComponent = GetComponent<EnemyHealthComponent>();

        Node root = new Selector(new List<Node>
        {

            new Sequence(new List<Node>
            { 
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new SporeGrowthTask(healthComponent),
                            new TaskCheckDistance(_unit, _target, 1f),
                            new StopFollowingTargetTask(_unit, _animator),
                            new WeaponAttackTask(_unit, _weapon, _target, _animator),
                        }),
                        new StartFollowingTargetTask(_target, _unit, _animator),
                    }),

                    new Sequence(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new WaitTaskBernas(spawnCooldown),
                            new SporeGoonTask(healthComponent),

                        }),

                        new Sequence(new List<Node>
                        {
                            //new WaitTask(throwCooldown),
                            new StopFollowingTargetTask(_unit, _animator),
                            new SporeLineTask(),
                            new SporeThrowTask(_unit, _weapon2, _animator),
                            new SporeLineTask(),
                            new SporeThrowTask(_unit, _weapon2, _animator),
                            new SporeLineTask(),
                            new SporeThrowTask(_unit, _weapon2, _animator),
                            new StartFollowingTargetTask(_target, _unit, _animator),
                        }),
                    }),
                    }),
        });

        root.SetData("target", GameObject.FindGameObjectWithTag("Player").transform);

        return root;
    }
}