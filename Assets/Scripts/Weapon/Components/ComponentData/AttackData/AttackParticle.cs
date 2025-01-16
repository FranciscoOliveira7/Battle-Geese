using System;
using UnityEngine;
using UnityEngine.VFX;

[Serializable]
public class AttackParticle : AttackData
{
    [field: SerializeField] public ParticleSystem ParticleObject;
}
