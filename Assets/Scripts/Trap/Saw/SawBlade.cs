using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SawBlade : MonoBehaviour
{
    public float speed = 2f; // Speed of the sawblade movement
    public float range = 1f; // Range of movement on the X-axis
    public float rotationSpeed = 180f; // Rotation speed of the sawblade
    public float damage = 2f; // Damage dealt by the sawblade
    public GameObject centralObject; // Reference to the central GameObject

    private Vector3 startPosition;
    private Rigidbody rb;
    private bool movingRight = true;

    private void Awake() => rb = GetComponent<Rigidbody>();

    private void Start()
    {
        if (PhotonNetwork.IsConnected && PhotonView.Get(this).Owner == null)
        {
            Server.Instance.InstantiateGameObject(gameObject.name, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (centralObject != null)
        {
            startPosition = centralObject.transform.position;
        }
        else
        {
            startPosition = transform.localPosition;
        }

        rb.velocity = centralObject.transform.right * speed;
        rb.AddTorque(transform.forward * rotationSpeed);
    }

    private void Update()
    {
        Vector3 localPosition = centralObject.transform.InverseTransformPoint(transform.position);

        if (movingRight && localPosition.x >= range)
        {
            movingRight = false;
            rb.velocity = -centralObject.transform.right * speed;
        }
        else if (!movingRight && localPosition.x <= -range)
        {
            movingRight = true;
            rb.velocity = centralObject.transform.right * speed;
        }
    }

    private void FixedUpdate()
    {
        // Ensure the saw blade keeps moving in the correct direction
        if (movingRight)
        {
            rb.velocity = centralObject.transform.right * speed;
        }
        else
        {
            rb.velocity = -centralObject.transform.right * speed;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(damage, Vector3.zero, null);
        }
    }
}


