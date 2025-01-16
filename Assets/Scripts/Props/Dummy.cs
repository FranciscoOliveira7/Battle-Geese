using Photon.Pun;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Dummy : MonoBehaviour, IDamageable
{
    [SerializeField] private ParticleSystem _damageParticles;
    private Animator _animator;

    public void Damage(float amount, Vector3 direction, HealthComponent source)
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _animator.SetTrigger("damage");
        _damageParticles.Play();
    }
}
