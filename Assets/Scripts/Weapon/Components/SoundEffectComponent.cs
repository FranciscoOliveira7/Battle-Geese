using UnityEngine;
using UnityEngine.VFX;

public class SoundEffectComponent : WeaponComponent<SoundEffectData, AttackSoundEffect>
{
    AudioSource _lastAudioSource;
    
    private void PlaySoundEffect()
    {
        if (!currentAttackData.isSoundLooping)
            SoundManager.instance.PlayClipPitched(currentAttackData.HitSoundClip, transform, currentAttackData.SoundVolume, currentAttackData.SoundPitch);
        else 
            _lastAudioSource = SoundManager.instance.PlayClipPitchedLoop(currentAttackData.HitSoundClip, transform, currentAttackData.SoundVolume, currentAttackData.SoundPitch);
    }
    
    private void PlaySoundEffect2()
    {
        if (currentAttackData.HitSoundClip2 == null) return;
        SoundManager.instance.PlayClipPitched(currentAttackData.HitSoundClip2, transform, currentAttackData.SoundVolume2, currentAttackData.SoundPitch2);
    }

    private void StopSoundEffect()
    {
        if (_lastAudioSource == null) return;
        Destroy(_lastAudioSource);
    }
    
    public override void Init()
    {
        base.Init();
        
        StopSoundEffect();
    }

    protected override void Start()
    {
        base.Start();

        EventHandler.OnHitSound += PlaySoundEffect;
        EventHandler.OnHitSound2 += PlaySoundEffect2;
        
        EventHandler.OnHitSoundStop += StopSoundEffect;
    }

    protected override void OnDestroy()
    {
        EventHandler.OnHitSound -= PlaySoundEffect;
        EventHandler.OnHitSound2 -= PlaySoundEffect2;
        
        EventHandler.OnHitSoundStop -= StopSoundEffect;
        
        StopSoundEffect();
        
        base.OnDestroy();
    }
}
