using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.VFX;

public class WaveProjectileScript : BasePorjectile
{

    public float Radius;
    public GameObject ExplosionVFX;
    VisualEffect waveVFXInstance;
    private Rigidbody rb;

    public AudioClip clip;
    public Transform soundTransform;
    public float startTime;
    public float duration;
    public float volume;

    public float Delay;

    public override void Spawn(Vector3 position, Vector3 target, float speed, float damage, int layerMask)
    {
        base.Spawn(position, target, speed, damage, layerMask);
        rb = GetComponent<Rigidbody>();
        Vector3 direction = (target - position).normalized;
        transform.position += direction;
        rb.velocity = (target - origin).normalized * speed;
        StartCoroutine(shoot());

        SoundManager.instance.PLayWithStartEnd(clip, soundTransform, startTime, duration, volume);
    }
    protected override void Hit(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            if (!PhotonNetwork.IsConnectedAndReady)
            {
                damageable.Damage(damage, (target - origin).normalized, sender);
            }
            else
            {
                other.GetComponent<PhotonView>()?.RPC("Damage",
                    RpcTarget.All,
                    damage,
                    (target - origin).normalized,
                    null);
            }
        }
    }

    private IEnumerator shoot()
    {
        waveVFXInstance = Instantiate(ExplosionVFX, transform.position, Quaternion.identity).GetComponent<VisualEffect>();
        waveVFXInstance.transform.forward = target - origin;
        transform.forward = target - origin;

        // explosionVFXInstance.Play();
        yield return new WaitForSeconds(Delay);

    }
}
