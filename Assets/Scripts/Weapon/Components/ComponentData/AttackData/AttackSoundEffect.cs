using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

[Serializable]
public class AttackSoundEffect : AttackData
{
    public bool isSoundLooping;
    [field: SerializeField] public float SoundVolume;
    [field: SerializeField] public float SoundPitch = 1f;
    [field: SerializeField] public AudioClip HitSoundClip;
    
    [field: SerializeField] public float SoundVolume2;
    [field: SerializeField] public float SoundPitch2 = 1f;
    [field: SerializeField] public AudioClip HitSoundClip2;
}
