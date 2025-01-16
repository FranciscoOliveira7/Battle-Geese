using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeStatus : WeaponComponent
{
    private Animator _animator;

    private void SetAnimatorParameter(bool value)
    {
        _animator.SetBool("charged", value);
    }

    protected override void Start()
    {
        base.Start();

        _animator = weapon.GetBase().GetComponent<Animator>();

        EventHandler.OnEnterAttackPhase += HandlePhase;
        EventHandler.OnAttack += HandleAttack;
    }

    private void HandleAttack()
    {
        SetAnimatorParameter(false);
    }

    private void HandlePhase(AttackPhases phase)
    {
        if (phase == AttackPhases.Charged)
        {
            SetAnimatorParameter(true);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventHandler.OnEnterAttackPhase -= HandlePhase;
        EventHandler.OnAttack -= HandleAttack;
    }
}