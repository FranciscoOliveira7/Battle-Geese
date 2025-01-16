using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEComponent : WeaponComponent<AOEComponentData, AttackAOE>
{
    private GameObject _aoeInstance;
    private bool isAEOSpawned;
    
    protected override void Start()
    {
        base.Start();

        EventHandler.OnAOESpawn += SpawnAEO;
        EventHandler.OnAttack += Attack;
    }

    private void Attack()
    {
        DestroyAOE();
    }

    private void SpawnAEO()
    {
        DestroyAOE();

        _aoeInstance = Instantiate(currentAttackData.AOE, weapon.TargetPosition, Quaternion.identity);
        isAEOSpawned = true;
    }

    private void Update()
    {
        if (isAEOSpawned)
        {
            _aoeInstance.transform.position = weapon.TargetPosition;
        }
    }
    
    private void DestroyAOE()
    {
        if (isAEOSpawned)
        {
            isAEOSpawned = false;
            Destroy(_aoeInstance);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        EventHandler.OnAOESpawn -= SpawnAEO;
        EventHandler.OnAttack -= Attack;

        DestroyAOE();
    }
}
