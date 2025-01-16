using UnityEngine;
using UnityEngine.VFX;

public class ParticleComponent : WeaponComponent<ParticleComponentData, AttackParticle>
{
    ParticleSystem _particlesInstance;
    
    private void SpawnParticles()
    {
        _particlesInstance = Instantiate(currentAttackData.ParticleObject, transform.position + Vector3.right * (int)weapon.Facing, Quaternion.identity);
        _particlesInstance.Play();
        Destroy(_particlesInstance.gameObject, 4f);
    }

    protected override void Start()
    {
        base.Start();

        EventHandler.OnAttack += SpawnParticles;
    }

    protected override void OnDestroy()
    {
        EventHandler.OnAttack -= SpawnParticles;
        
        base.OnDestroy();
    }
}
