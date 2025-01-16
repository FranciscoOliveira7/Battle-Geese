using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponGenerator : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    public WeaponDataSO Data;

    private List<WeaponComponent> componentsAlreadyOnWeapon = new();
    private List<WeaponComponent> componentsAddedToWeapon = new();
    private List<Type> componentDependencies = new();

    private Animator _animator;

    private void Awake()
    {
        _animator = _weapon.GetBase().GetComponent<Animator>();
        if (Data != null)
        {
            GenerateWeapon(Data);
        }
    }

    [ContextMenu("Test Generate")]
    private void TestGeneration()
    {
        if (Data != null)
        {
            GenerateWeapon(Data);
            _weapon.Equip();
        }
    }

    // public void GenerateWeapon(WeaponDataSO data)
    // {
    //     if (data == null)
    //     {
    //         Debug.LogWarning("Weapon data is not assigned.");
    //         return;
    //     }
    //
    //     if (_weapon == null)
    //     {
    //         Debug.LogWarning("Weapon is not assigned.");
    //         return;
    //     }
    //
    //     _weapon.SetData(data);
    //
    //     componentsAddedToWeapon.Clear();
    //     componentDependencies.Clear();
    //
    //     componentDependencies = data.GetAllDependencies();
    //     
    //     var componentsToRemove = GetComponents<WeaponComponent>().ToList();
    //
    //     foreach (var weaponComponent in componentsToRemove)
    //     {
    //         Destroy(weaponComponent);
    //     }
    //
    //     foreach (var dependency in componentDependencies)
    //     {
    //         if (componentsAddedToWeapon.FirstOrDefault(component => component.GetType() == dependency.GetType()))
    //         {
    //             var tmpComponent = componentsAlreadyOnWeapon.FirstOrDefault(component => component.GetType() == dependency);
    //             continue;
    //         }
    //
    //         var weaponComponent =
    //             componentsAlreadyOnWeapon.FirstOrDefault(component => component.GetType() == dependency);
    //
    //         if (weaponComponent == null)
    //         {
    //             weaponComponent = gameObject.AddComponent(dependency) as WeaponComponent;
    //         }
    //
    //         weaponComponent.Init();
    //         componentsAddedToWeapon.Add(weaponComponent);
    //     }
    //     
    //     // _weapon.SetSprite(data.AnimatorController);
    //     
    //     _animator.runtimeAnimatorController = data.AnimatorController;
    // }
    
    public void GenerateWeapon(WeaponDataSO data)
    {
        if (data == null)
        {
            Debug.LogWarning("Weapon data is not assigned.");
            return;
        }
    
        if (_weapon == null)
        {
            Debug.LogWarning("Weapon is not assigned.");
            return;
        }
    
        _weapon.SetData(data);
    
        componentsAlreadyOnWeapon.Clear();
        componentsAddedToWeapon.Clear();
        componentDependencies.Clear();
    
        componentsAlreadyOnWeapon = GetComponents<WeaponComponent>().ToList();
        componentDependencies = data.GetAllDependencies();
    
        foreach (var dependency in componentDependencies)
        {
            if (componentsAddedToWeapon.FirstOrDefault(component => component.GetType() == dependency.GetType()))
                continue;
    
            var weaponComponent =
                componentsAlreadyOnWeapon.FirstOrDefault(component => component.GetType() == dependency);
    
            if (weaponComponent == null)
            {
                weaponComponent = gameObject.AddComponent(dependency) as WeaponComponent;
            }
    
            weaponComponent.Init();
            componentsAddedToWeapon.Add(weaponComponent);
        }
    
        var componentsToRemove = componentsAlreadyOnWeapon.Except(componentsAddedToWeapon);
    
        foreach (var weaponComponent in componentsToRemove)
        {
            Destroy(weaponComponent);
        }
        
        // _weapon.SetSprite(data.AnimatorController);
        
        _animator.runtimeAnimatorController = data.AnimatorController;
    }
}
