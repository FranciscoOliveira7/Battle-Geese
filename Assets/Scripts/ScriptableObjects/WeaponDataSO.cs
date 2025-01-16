using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Basic Weapon Data", order = 0)]
public class WeaponDataSO : ScriptableObject
{
    public WeaponDataSO()
    {
        ComponentDatas = new List<ComponentData>();
    }
    public static WeaponDataSO Instances => Resources.LoadAll<WeaponDataSO>("Data/Weapons").FirstOrDefault();

    [field: SerializeField] public string Name { get; set; }
    [Multiline(4)]
    public string Description;
    //[field: SerializeField] public string Description { get; set; }
    [field: SerializeField] public RuntimeAnimatorController AnimatorController { get; set; }
    [field: SerializeField] public Sprite WeaponSprite { get; set; }
    [field: SerializeField] public GameObject WeaponPrefab { get; set; } // Add this line
    [field: SerializeReference] public List<ComponentData> ComponentDatas { get; set; }

    public T GetData<T>()
    {
        return ComponentDatas.OfType<T>().FirstOrDefault();
    }

    public List<Type> GetAllDependencies()
    {
        return ComponentDatas.Select(component => component.ComponentDependency).ToList();
    }

    public void AddData(ComponentData data)
    {
        if (ComponentDatas.FirstOrDefault(t => t.GetType() == data.GetType()) != null) return;

        ComponentDatas.Add(data);
    }
}
