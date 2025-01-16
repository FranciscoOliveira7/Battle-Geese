using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHold : WeaponComponent
{
    private Animator _animator;

    private bool _input;

    private void HandleInputChange(bool newInput)
    {
        _input = newInput;

        SetAnimatorParameter();
    }

    private void SetAnimatorParameter()
    {
        _animator.SetBool("hold", _input);
    }

    public override void Init()
    {
        base.Init();
        
        // _animator.SetBool("hold", false);
    }

    protected override void Start()
    {
        base.Start();

        _animator = weapon.GetBase().GetComponent<Animator>();

        weapon.OnCurrentInputChange += HandleInputChange;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        weapon.OnCurrentInputChange -= HandleInputChange;
    }
}
