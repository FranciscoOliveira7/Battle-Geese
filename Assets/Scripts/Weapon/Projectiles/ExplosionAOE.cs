using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class ExplosionAOE : BasePorjectile
{
    public float Radius;
    public GameObject ExplosionVFX;
    VisualEffect explosionVFXInstance;
    private CinemachineImpulseSource _impulseSource;
    
    public float Delay;
    
    [PunRPC]
    public override void Spawn(Vector3 position, Vector3 target, float speed, float damage, int layerMask)
    {
        base.Spawn(position, target, speed, damage, layerMask);
        
        _impulseSource = GetComponent<CinemachineImpulseSource>();

        transform.position = target;

        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        explosionVFXInstance = Instantiate(ExplosionVFX, transform).GetComponent<VisualEffect>();
        // explosionVFXInstance.Play();
        yield return new WaitForSeconds(Delay);
        
        CameraShakeManager.instance.CameraShake(_impulseSource);
        DamageUtil.DamageInRadius(transform.position, Radius, damage, layerMask);
        
        Destroy(gameObject, 3.5f);
    }
}
