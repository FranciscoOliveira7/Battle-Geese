using System;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public event Action<Collider> OnCollision;
    public SpikeTrapStateMachine SpikeTrapStateMachine { get; private set; }

    public AudioClip _trapSound;


    public BoxCollider BoxCollider { get; private set; }

    public float CooldownTime = 2f;
    public float Damage = 20f;

    public Sprite inactiveSprite;
    public SpriteRenderer spriteRenderer;

    private void OnTriggerStay(Collider other)
    {
        OnCollision?.Invoke(other);
    }
    public void PlayAudio()
    {
        SoundManager.instance.PlayClip(_trapSound, transform, 1);
    }

    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        SpikeTrapStateMachine = new SpikeTrapStateMachine(this);
        SpikeTrapStateMachine.Initialize(SpikeTrapStateMachine.ActiveState);
    }

    private void Update()
    {
        SpikeTrapStateMachine.Update();
    }
}
