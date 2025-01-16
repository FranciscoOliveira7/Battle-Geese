using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;
using Tree = BehaviourTree.Tree;

public class TofuBrothersBT : Tree
{
    [Header("Tofu Brothers")]
    private Unit tofuBrother1;
    private Unit tofuBrother2;
    [SerializeField] private float swapInterval = 10f;
    [SerializeField] Transform Avoidant;
    [SerializeField] private LayerMask wallLayerMask; // Layer mask to detect walls

    private TaskSwapRoles _taskSwapRoles;
    private TaskUpdateAvoidantPosition _taskUpdateAvoidantPosition;
    private TaskCheckCollision _checkCollisionTask1;
    private TaskCheckCollision _checkCollisionTask2;
    private TaskAgressive _taskAgressive1;
    private TaskAgressive _taskAgressive2;
    private TaskAvoid _taskAvoid1;
    private TaskAvoid _taskAvoid2;
    private Animator _animator1;
    private Animator _animator2;

    private void Awake()
    {
        InitializeTofuBrothers();
        EquipInitialWeapons();
        SetInitialTargets();
        InitializeTasks();
    }

    private void InitializeTofuBrothers()
    {
        tofuBrother1 = transform.Find("TofuBrother1").GetComponent<Unit>();
        tofuBrother2 = transform.Find("TofuBrother2").GetComponent<Unit>();
        _animator1 = tofuBrother1.GetComponent<Animator>();
        _animator2 = tofuBrother2.GetComponent<Animator>();
    }

    private void EquipInitialWeapons()
    {
        Weapon primaryWeapon1 = tofuBrother1.transform.Find("PrimaryWeapon").GetComponent<Weapon>();
        Weapon secondaryWeapon1 = tofuBrother1.transform.Find("SecondaryWeapon").GetComponent<Weapon>();
        Weapon primaryWeapon2 = tofuBrother2.transform.Find("PrimaryWeapon").GetComponent<Weapon>();
        Weapon secondaryWeapon2 = tofuBrother2.transform.Find("SecondaryWeapon").GetComponent<Weapon>();

        if (primaryWeapon1 != null) primaryWeapon1.Equip();
        if (secondaryWeapon1 != null) secondaryWeapon1.Unequip();
        if (primaryWeapon2 != null) primaryWeapon2.Unequip();
        if (secondaryWeapon2 != null) secondaryWeapon2.Equip();
    }

    private void SetInitialTargets()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            tofuBrother1.movingTarget = player;
            tofuBrother2.movingTarget = Avoidant;
        }
    }

    private void InitializeTasks()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("TofuBrothersBT: Player not found.");
            return;
        }

        if (Avoidant == null)
        {
            Debug.LogError("TofuBrothersBT: Avoidant not found.");
            return;
        }

        Weapon primaryWeapon1 = tofuBrother1.transform.Find("PrimaryWeapon").GetComponent<Weapon>();
        Weapon secondaryWeapon1 = tofuBrother1.transform.Find("SecondaryWeapon").GetComponent<Weapon>();
        Weapon primaryWeapon2 = tofuBrother2.transform.Find("PrimaryWeapon").GetComponent<Weapon>();
        Weapon secondaryWeapon2 = tofuBrother2.transform.Find("SecondaryWeapon").GetComponent<Weapon>();

        _taskAgressive1 = new TaskAgressive(tofuBrother1, player, primaryWeapon1, _animator1);
        _taskAgressive2 = new TaskAgressive(tofuBrother2, player, primaryWeapon2, _animator2);
        //_taskAvoid1 = new TaskAvoid(tofuBrother1, Avoidant, player, secondaryWeapon1);
        //_taskAvoid2 = new TaskAvoid(tofuBrother2, Avoidant, player, secondaryWeapon2);

        _taskSwapRoles = new TaskSwapRoles(tofuBrother1, tofuBrother2, Avoidant, swapInterval, wallLayerMask, _taskAgressive1, _taskAgressive2, _taskAvoid1, _taskAvoid2);
        //_taskUpdateAvoidantPosition = new TaskUpdateAvoidantPosition(_taskSwapRoles, Avoidant, wallLayerMask);
        _checkCollisionTask1 = new TaskCheckCollision(tofuBrother1.transform);
        _checkCollisionTask2 = new TaskCheckCollision(tofuBrother2.transform);
    }

    protected override Node SetupTree()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("TofuBrothersBT: Player not found.");
            return null;
        }

        Node root = new Selector(new List<Node>
        {
            new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    _taskAgressive1,
                    _taskAvoid1
                }),
                new Sequence(new List<Node>
                {
                    _taskAvoid2,
                    _taskAgressive2
                })
            })
        });

        return root;
    }

    private void Update()
    {
        if (tofuBrother1 == null || tofuBrother2 == null)
        {
            return;
        }

        _root.Evaluate();
        _taskSwapRoles.Evaluate();
        _checkCollisionTask1.SetData("target", tofuBrother1.movingTarget);
        _checkCollisionTask1.Evaluate();
        _checkCollisionTask2.SetData("target", tofuBrother2.movingTarget);
        _checkCollisionTask2.Evaluate();
        _taskUpdateAvoidantPosition.Evaluate();
    }
}
