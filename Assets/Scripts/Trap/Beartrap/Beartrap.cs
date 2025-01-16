using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beartrap : MonoBehaviour
{
    public event Action<Collider> OnCollision;
    public BeartrapStateMachine BeartrapStateMachine { get; private set; }

    [SerializeField] private AudioClip _trapSound;
    public BoxCollider BoxCollider { get; private set; }

    public float CooldowndTime = 5f;
    public float Damage = 20f;

    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;


    private void OnTriggerStay(Collider other)
    {
        OnCollision?.Invoke(other);
    }
    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        BeartrapStateMachine = new BeartrapStateMachine(this);
        BeartrapStateMachine.Initialize(BeartrapStateMachine.ActiveState);

    }

    // Update is called once per frame
    void Update()
    {
        BeartrapStateMachine.Update();
    }
}
