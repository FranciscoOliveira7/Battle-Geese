using System;
using UnityEngine;

public class ActionHitbox : WeaponComponent<ActionHitboxData, AttackActionHitbox>
{
    private int _facingDirection = 1;

    private Vector3 _offset;

    private Collider[] detected;

    public event Action<Collider[]> OnDetectedCollider;

    private void HandleAttackAction()
    {
        _offset.Set(
            transform.position.x + (currentAttackData.Hitbox.center.x * (int)weapon.Facing),
            transform.position.y,
            transform.position.z + currentAttackData.Hitbox.center.y
        );

        detected = Physics.OverlapBox(_offset,
            new Vector3(data.AttackData.Hitbox.width, 1f, data.AttackData.Hitbox.height) / 2,
            transform.rotation, data.DetectableLayers);

        if (detected.Length == 0) return;

        OnDetectedCollider?.Invoke(detected);
    }

    protected override void Start()
    {
        base.Start();
        
        _facingDirection = (int)weapon.Facing;

        EventHandler.OnAttack += HandleAttackAction;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventHandler.OnAttack -= HandleAttackAction;
    }

    private void OnDrawGizmosSelected()
    {
        if (data == null || !data.AttackData.Debug) return;

        Gizmos.DrawWireCube(new Vector3(transform.position.x + (data.AttackData.Hitbox.center.x * (int)weapon.Facing),
            transform.position.y,
            transform.position.z + data.AttackData.Hitbox.center.y),
            new Vector3(data.AttackData.Hitbox.width, 0.1f, data.AttackData.Hitbox.height));
    }
}