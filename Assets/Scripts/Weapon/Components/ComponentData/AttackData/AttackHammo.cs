using System;
using UnityEngine;

[Serializable]
public class AttackHammo : AttackData
{
    [field: SerializeField] public int MaxHammo { get; private set; }
    [field: SerializeField] public float TimeToRegen1 { get; private set; }
}
