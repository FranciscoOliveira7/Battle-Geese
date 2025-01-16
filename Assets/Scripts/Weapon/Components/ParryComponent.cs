
using Photon.Pun;
using UnityEngine;

public class ParryComponent : WeaponComponent<ParryComponentData, AttackParry>
{
    private ActionHitbox _hitbox;

    private void HandleDetectCollider(Collider[] colliders)
    {
        foreach (var collider in colliders)
        {
            if(collider.TryGetComponent(out BasePorjectile projectile))
            {
                Parry(projectile);
            }
        }
    }

    private void Parry(BasePorjectile projectile)
    {
        SoundManager.instance.PlayClip(currentAttackData.parrySound, transform, 0.5f);
        GameObject effect = Instantiate(currentAttackData.parryEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2f);
        
        projectile.rigidbody.velocity = -projectile.rigidbody.velocity;
        projectile.rigidbody.excludeLayers -= 1 << 8;
        projectile.rigidbody.excludeLayers += 1 << 3;
        // Debug.Log(projectile.sender);
        // projectile.rigidbody.AddForce(
        //     (projectile.sender.transform.position - transform.position).normalized * projectile.speed,
        //     ForceMode.Impulse);
    }

    protected override void Awake()
    {
        base.Awake();

        _hitbox = GetComponent<ActionHitbox>();
    }

    protected override void Start()
    {
        base.Start();

        _hitbox.OnDetectedCollider += HandleDetectCollider;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _hitbox.OnDetectedCollider -= HandleDetectCollider;
    }
}