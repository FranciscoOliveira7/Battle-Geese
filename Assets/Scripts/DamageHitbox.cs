using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHitbox : MonoBehaviour
{
    [SerializeField] private float _damage;
    // [SerializeField] private float _damageRadius;
    [SerializeField] private float _damageCooldown = 0.8f;
    // public DamageType Da = DamageType.None;
    // public LayerMask layerMask;
    private float _timer;
    private bool hasDamaged;
    private void OnTriggerStay(Collider other)
    {
        if (hasDamaged) return;

        if (other.TryGetComponent(out IDamageable yes))
        {
            yes.Damage(_damage, other.transform.position - transform.position, null);
            hasDamaged = true;
            _timer = 0;

        }
    }


    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer < _damageCooldown) return;

        hasDamaged = false;
    }


    // private void OnTriggerEnter(Collider other)
    // {
    //     Damage(other);
    // }
    //
    // private void Damage(Collider other)
    // {
    //     if (other.TryGetComponent(out IDamageable player))
    //     {
    //         player.Damage(_damage, other.transform.position - transform.position);
    //     }
    // }
}
