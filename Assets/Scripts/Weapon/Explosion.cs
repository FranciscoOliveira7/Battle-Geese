using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AnimationEventHandler _eventHandler;

    private void Awake()
    {
        _audioSource.Play();
        
        _eventHandler.OnFinish += DestroyThis;
    }

    private void OnDestroy()
    {
        _eventHandler.OnFinish -= DestroyThis;
    }
    
    private void DestroyThis() => Destroy(this.gameObject);
}
