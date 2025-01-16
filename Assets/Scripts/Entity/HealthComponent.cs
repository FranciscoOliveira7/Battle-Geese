using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthComponent : MonoBehaviour, IDamageable
{
    public event Action OnDeath;
    public event Action<float> OnDamageReceived;
    public event Action OnHealthUpdated;
    public float dmgMultiplier = 1f;
    private SpriteRenderer _spriteRenderer;
    private Material _defaultMaterial;
    [SerializeField] private Material _damagedMaterial;
    [SerializeField] private ParticleSystem _damagedParticles;
    [SerializeField] private AudioClip[] _damagedSound;
    [SerializeField] private float _damagedSoundVolume = 0.2f;
    [SerializeField] private Color _damagedParticleColor = Color.white;
    
    private ParticleSystem _damagedParticlesInstance;

    [HideInInspector] public float LifeSteal;
    
    public PhotonView photonView { get; private set; }

    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            OnHealthUpdated?.Invoke();
        }
    }
    public float MaxHealth
    {
        get => _maxHealth;
        set
        {
            _maxHealth = value;
            Health += value;
            Health = Mathf.Clamp(Health, 0, _maxHealth);
        }
    }
    
    [SerializeField] private float _health;
    private float _maxHealth;
    protected bool _isDead;

    private void Awake()
    {
        photonView = PhotonView.Get(this);
        if (Health <= 0)
        {
            Debug.LogWarning("U forgor to add health to: " + gameObject.name +
                             "\nBut no worries, I set it to 999", gameObject);
            Health = 999;
        }
        _maxHealth = Health;
    }
    
    private void Start()
    {
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;
    }

    public void Revive()
    {
        Health = MaxHealth;
        _isDead = false;
    }
    
    [PunRPC]
    public virtual void Damage(float amount, Vector3 direction, HealthComponent source)
    {
        Health -= amount * dmgMultiplier;

        if (source != null)
        {
            source.Health += amount * source.LifeSteal;
        }
        
        Health = Mathf.Clamp(_health, 0, _maxHealth);
        
        OnDamageReceived?.Invoke(amount);
        if (Health <= 0 && !_isDead) Die();

        DamageFeedback(direction);
    }
    protected virtual void Die()
    {
        _isDead = true;
        OnDeath?.Invoke();
    }

    private void DamageFeedback(Vector3 attackDirection)
    {
        SoundManager.instance.PlayRandomClipWithRandomPitch(_damagedSound, transform, _damagedSoundVolume, 0.9f, 1f);
        SpawnParticles(attackDirection);
        StartCoroutine(nameof(SpriteFlash));
    }

    private void SpawnParticles(Vector3 attackDirection)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.right, attackDirection);
        
        _damagedParticlesInstance = Instantiate(_damagedParticles, transform.position, spawnRotation);
        Renderer renderer = _damagedParticlesInstance.GetComponent<Renderer>();
        renderer.material.color = _damagedParticleColor;
        _damagedParticlesInstance.Play();
    }

    private IEnumerator SpriteFlash()
    {
        Renderer renderer = _spriteRenderer.GetComponent<Renderer>();
        _spriteRenderer.material = _damagedMaterial;
        renderer.material.SetColor("_EmissionColor", Color.red);
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.material = _defaultMaterial;
    }
}
