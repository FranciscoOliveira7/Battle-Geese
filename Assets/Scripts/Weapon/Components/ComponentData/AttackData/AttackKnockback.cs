using System;
using UnityEngine;

[Serializable]
public class AttackKnockback : AttackData
{
    [field: SerializeField] public int Amount { get; private set; }
}
