using System;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;
using Tree = BehaviourTree.Tree;

public class AniBt : Tree
{
    private Transform _target;
    private Weapon _weapon;
    private Unit _unit;
    private Animator _animator;
    public static float attackRange = 6f;
    public static float attackCooldown = 1f;
    public static string attackType = "normal";
    public static bool isMoving = false;



    protected override Node SetupTree()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _unit = GetComponent<Unit>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _unit.movingTarget = _target;
        _weapon = transform.Find("PrimaryWeapon").GetComponent<Weapon>();
        _weapon.Equip();
        Debug.Log("Ani", _weapon.gameObject);
        EnemyHealthComponent healthComponent = GetComponent<EnemyHealthComponent>();

        Node root = new Selector(new List<Node>
        {
            //select between (sequence for CheckIsAgressive and Sequence Agressive) and Sequence for Avoid
           new Selector(new List<Node>
           {
               new Sequence(new List<Node>
               {
                   //check is agressive
                   /*new CheckIsAgressive(healthComponent),*/
                   //sequence agressive
                   new Sequence(new List<Node>
                   {
                       //follow target
                      new StartFollowingTargetTask(_target, _unit, _animator),
                       //Dash
                      new DashAttackTask(_unit, 2f, 0.4f, 15, _animator),
                       //Melee Attack
                      new WeaponAttackTask(_unit, _weapon, _target, _animator),
                   }),
               }),
               //avoid
                new Sequence(new List<Node>
                {
                    
                         //Follow Avoidant
                         //Shoot Attack
                         //Shoot CoolDown
                     
                }),
           }),
        });

        root.SetData("target", GameObject.FindGameObjectWithTag("Player").transform);

        return root;
    }
}
