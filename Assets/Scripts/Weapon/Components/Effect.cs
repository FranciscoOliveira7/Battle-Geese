using System;
using UnityEngine;
using UnityEngine.VFX;

public class Effect : WeaponComponent<EffectData, AttackEffect>
{
    VisualEffect VisualEffect;
    GameObject visualEffectObject;

    private void PlayVisualEffect() => VisualEffect.Play();

    private void StopAnimation() => VisualEffect.Stop();

    public override void Init()
    {
        base.Init();
        
        DestroyVisualEffect();

        visualEffectObject = Instantiate(data.AttackData.EffectObject, transform);

        VisualEffect = visualEffectObject.GetComponent<VisualEffect>();
    }
    
    private void DestroyVisualEffect()
    {
        if (visualEffectObject != null)
        {
            Destroy(visualEffectObject);
        }
    }

    protected override void Start()
    {
        base.Start();

        EventHandler.OnVFXStart += PlayVisualEffect;
        EventHandler.OnVFXStop += StopAnimation;
    }

    protected override void OnDestroy()
    {
        EventHandler.OnVFXStart -= PlayVisualEffect;
        EventHandler.OnVFXStop -= StopAnimation;
        
        DestroyVisualEffect();
        
        base.OnDestroy();
    }

    private void Update()
    {
        if (!data.AttackData.shouldFlip) visualEffectObject.transform.localScale = new Vector3((int)weapon.Facing, 1, 1);
    }
}
