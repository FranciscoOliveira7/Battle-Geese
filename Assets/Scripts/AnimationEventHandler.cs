using System;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public event Action OnFinish;
    public event Action OnStep;
    public event Action OnHitSound;
    public event Action OnHitSoundStop;
    public event Action OnHitSound2;
    public event Action OnAttack;
    public event Action OnAttack2;
    public event Action OnAOESpawn;
    public event Action OnVFXStart;
    public event Action OnVFXStop;
    public event Action<AttackPhases> OnEnterAttackPhase;

    private void AnimationFinishTrigger() => OnFinish?.Invoke();
    private void StepTrigger() => OnStep?.Invoke();
    private void AttackTrigger() => OnAttack?.Invoke();
    private void Attack2Trigger() => OnAttack2?.Invoke();
    private void AOESpawnTrigger() => OnAOESpawn?.Invoke();
    private void VFXStartTrigger() => OnVFXStart?.Invoke();
    private void VFXStopTrigger() => OnVFXStop?.Invoke();
    private void HitSoundTrigger() => OnHitSound?.Invoke();
    private void HitSoundStopTrigger() => OnHitSoundStop?.Invoke();
    private void HitSoundTrigger2() => OnHitSound2?.Invoke();
    private void EnterAttackPhase(AttackPhases phase) => OnEnterAttackPhase?.Invoke(phase);
}
