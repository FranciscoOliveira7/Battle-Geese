using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LingeringZone : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private AnimationCurve _animationCurve;
    SpriteRenderer _spriteRenderer;
    private float _timer;
    private Color _spriteColor;

    private void Awake()
    {
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _spriteColor = _spriteRenderer.color;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        
        _spriteColor.a = Mathf.Lerp(1f, _animationCurve.Evaluate(_timer), _timer / _duration);
        
        _spriteRenderer.color = _spriteColor;

        if (_timer >= _duration) Destroy(gameObject);
    }
}
