using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Prop : MonoBehaviour, IDamageable
{
    [SerializeField] private ParticleSystem _damageParticles;

    public void Damage(float amount, Vector3 direction, HealthComponent source)
    {
        _damageParticles.Play();
        // Play particle effect
        //Destroy(gameObject);
    }
}
