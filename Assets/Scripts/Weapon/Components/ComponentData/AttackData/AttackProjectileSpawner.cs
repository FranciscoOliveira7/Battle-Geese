using System;
using UnityEngine;

[Serializable]
public class AttackProjectileSpawner : AttackData
{
    [field: SerializeField] public GameObject Projectile { get; private set; }
}
