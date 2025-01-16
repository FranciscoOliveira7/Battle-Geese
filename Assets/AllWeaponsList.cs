using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "allWeaponsList", menuName = "Data/allWeaponsList", order = 0)]
public class AllWeaponsList : ScriptableObject
{
    public List<WeaponDataSO> AllWeapons = new List<WeaponDataSO>();
    
    public List<HatSO> AllHats = new List<HatSO>();
    
    public int GetWeaponIndex(WeaponDataSO weapon)
    {
        return AllWeapons.IndexOf(weapon);
    }
    
    public WeaponDataSO GetWeaponByIndex(int index)
    {
        return AllWeapons[index];
    }
    
    public int GetHatIndex(HatSO hat)
    {
        return AllHats.IndexOf(hat);
    }
    
    public HatSO GetHatByIndex(int index)
    {
        return AllHats[index];
    }
}
