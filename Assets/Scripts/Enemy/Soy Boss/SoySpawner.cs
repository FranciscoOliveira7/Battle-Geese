using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoySpawner : MonoBehaviour
{
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private float shootSoundVolume;
    public GameObject projectile;
    public LayerMask layerMask;

    public void Spawn(float speed, float damage)
    {
        // Vector3 direction = new(
        //     Random.Range(1f, 2f) * (float)Random.Range(0, 2) * 2 - 1,
        //     0f,
        //     Random.Range(1f, 2f) * (float)Random.Range(0, 2) * 2 - 1
        //     );
        Vector3 direction = Random.Range(-2.0f, 2.0f) * transform.forward + Random.Range(-2.0f, 2.0f) * transform.right;

        GameObject soy = Instantiate(projectile);

        soy.GetComponent<IProjectile>().Spawn(
            transform.position,
            transform.position + direction,
            speed,
            damage,
            layerMask
        );
        
        SoundManager.instance.PlayClipWithRandomPitch(shootSound, transform, shootSoundVolume, 0.9f, 1.1f);
    }
}
