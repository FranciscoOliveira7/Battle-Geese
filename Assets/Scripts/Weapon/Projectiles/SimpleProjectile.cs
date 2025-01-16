using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SimpleProjectile : BasePorjectile
{
    [SerializeField] private bool _isBuildBoard;
    [SerializeField] private bool _isPiercing;
    private Vector3 _direction;
    
    private Vector3 _velocity;
    private Transform _spriteTransform;

    [PunRPC]
    public override void Spawn(Vector3 position, Vector3 target, float speed, float damage, int layerMask)
    {
        base.Spawn(position, target, speed, damage, layerMask);
        
        Vector3 distance = target - position;
        
        _velocity = distance.normalized * speed;

        rigidbody.velocity = _velocity;

        if (_isBuildBoard) return;
        
        _spriteTransform = transform.Find("Sprite");

        _spriteTransform.transform.right = _velocity;
        
        _spriteTransform.transform.Rotate(Vector3.right * 90f);
    }
    
    protected override void Hit(Collider other)
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
        if (other.gameObject.layer == 1 << 10) Destroy(gameObject);
    }
}
