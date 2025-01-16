using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobShadow : MonoBehaviour
{
    public GameObject shadow;
    public Transform Transform;
    public float offset;

    private void Awake()
    {
        Transform = shadow.transform;
    }

    private void FixedUpdate()
    {
        shadow.transform.position = new Vector3(transform.position.x, offset, transform.position.z);
    }
}
