
using UnityEngine;

public class KnockbackComponent : WeaponComponent<KnockbackComponentData, AttackKnockback>
{
    private ActionHitbox _hitbox;

    private void HandleDetectCollider(Collider[] colliders)
    {
        foreach (var collider in colliders)
        {
            if(collider.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(
                    new ((int)weapon.Facing * currentAttackData.Amount, 0, 0),
                    ForceMode.Impulse);
            }
        }
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