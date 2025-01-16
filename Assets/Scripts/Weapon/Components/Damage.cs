
using Photon.Pun;
using UnityEngine;

public class Damage : WeaponComponent<DamageData, AttackDamage>
{
    private ActionHitbox _hitbox;

    private void HandleDetectCollider(Collider[] colliders)
    {
        foreach (var collider in colliders)
        {
            if(collider.TryGetComponent(out IDamageable damageable))
            {
                if (!PhotonNetwork.IsConnectedAndReady)
                {
                    damageable.Damage(
                        currentAttackData.Amount,
                        new Vector3((int)weapon.Facing, 0, 0),
                        weapon.User
                        );
                }
                else
                {
                    collider.GetComponent<PhotonView>()?.RPC("Damage",
                        RpcTarget.All,
                        currentAttackData.Amount,
                        new Vector3((int)weapon.Facing, 0, 0),
                        null);
                }
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