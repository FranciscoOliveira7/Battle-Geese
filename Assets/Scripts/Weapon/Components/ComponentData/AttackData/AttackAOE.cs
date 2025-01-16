using System;
using UnityEngine;

[Serializable]
public class AttackAOE : AttackData
{
    [field: SerializeField] public GameObject AOE { get; private set; }
}