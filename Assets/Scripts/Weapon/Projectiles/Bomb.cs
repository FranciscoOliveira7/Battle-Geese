using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class Bomb : BasePorjectile
{
    [SerializeField] private float radius;
    [SerializeField] private GameObject _explosionGameObject;
    
    private bool hasExploded;
    
    private float _duration;
    
    // x(t) = x + vxt
    // y(t) = y + vyt - ayt²
    private Vector3 _velocity;

    private float vy0;

    [PunRPC]
    public override void Spawn(Vector3 position, Vector3 target, float speed, float damage, int layerMask)
    {
        base.Spawn(position, target, speed, damage, layerMask);
        
        Vector3 distance = target - position;
        
        _velocity = distance.normalized * speed;
        
        // tempo = distancia / velocidade
        _duration = distance.magnitude / speed;
        
        // vy0
        _velocity.y = -Physics.gravity.y / 2 * _duration;
        
        rigidbody.velocity = _velocity;
    }

    private void Update()
    {
        if (transform.position.y < 0f) Explode();
    }

    private void Explode()
    {
        if (hasExploded) return;
        
        hasExploded = true;
        
        if (_explosionGameObject)
            Instantiate(_explosionGameObject, transform.position, transform.rotation);
        
        if (!PhotonNetwork.IsConnected || (PhotonNetwork.IsConnected && PhotonView.Get(this).IsMine))
            DamageUtil.DamageInRadius(transform.position, radius, damage, layerMask);
        transform.Find("Sprite").gameObject.SetActive(false);
        transform.Find("Particles").GetComponent<ParticleSystem>().Stop();
        Destroy(gameObject, 4);
    }

    protected override void Hit(Collider other) {}
}