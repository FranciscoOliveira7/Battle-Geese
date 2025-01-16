using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

[Serializable]
public class AttackEffect : AttackData
{
    [field: SerializeField] public GameObject EffectObject;
    [FormerlySerializedAs("shouldNotRotate")] [field: SerializeField] public bool shouldFlip;
}
