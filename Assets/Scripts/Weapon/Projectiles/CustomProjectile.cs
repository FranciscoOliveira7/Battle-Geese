using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomProjectile : BasePorjectile
{
    [SerializeField] Transform cosmeticProjectile;
    float _timeElapsed = 0f;

    private void Update()
    {
        _timeElapsed += Time.deltaTime;
        
        float y = origin.y + 2 * _timeElapsed - 9.8f * _timeElapsed * _timeElapsed;

        cosmeticProjectile.transform.position = new(cosmeticProjectile.transform.position.x,
            y, cosmeticProjectile.transform.position.x);
        
    }
}
