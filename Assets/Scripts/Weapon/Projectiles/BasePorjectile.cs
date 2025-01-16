using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public interface IProjectile
{
    public void Spawn(Vector3 position, Vector3 target, float speed, float damage, int layerMask);
}

public abstract class BasePorjectile : MonoBehaviour, IProjectile
{
    private const float lifeTime = 3f;

    protected Vector3 origin;
    protected float damage;
    protected Vector3 target;
    protected LayerMask layerMask;
    [HideInInspector] public float speed;
    [HideInInspector] public HealthComponent sender;
    [HideInInspector] public Rigidbody rigidbody;
    
    public virtual void Spawn(
        Vector3 position,
        Vector3 target,
        float speed,
        float damage,
        int layerMask)
    {
        this.speed = speed;
        transform.position = position;
        origin = position;
        this.damage = damage;
        this.target = target;
        this.layerMask = layerMask;

        rigidbody = GetComponent<Rigidbody>();
        
        rigidbody.excludeLayers = ~layerMask;

        Destroy(gameObject, lifeTime);
    }

    protected virtual void Hit(Collider other)
    {
        // Maybe add a particle effect later

        if (other.TryGetComponent(out IDamageable damageable))
        {
            if (!PhotonNetwork.IsConnectedAndReady)
            {
                damageable.Damage(damage, (target - origin).normalized, sender);
            }
            else if (PhotonView.Get(this).IsMine)
            {
                other.GetComponent<PhotonView>()?.RPC("Damage",
                    RpcTarget.All,
                    damage,
                    (target - origin).normalized,
                    null);
            }
        }
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other) => Hit(other);
}
