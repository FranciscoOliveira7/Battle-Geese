using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Hat : MonoBehaviour
{
    [SerializeField] protected Sprite sprite;
    protected Player player;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _playerSprite;
    
    public HatSO Data;
    
    private HatBehaviour _behaviour;

    [SerializeField] public HatBehaviour Behaviour;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerSprite = transform.parent.GetComponent<SpriteRenderer>();
        player = _playerSprite.transform.parent.GetComponent<Player>();
    }

    public void SetData(HatSO data) {
        Debug.Log("Hat");
        gameObject.AddComponent<HatBehaviour>();
        Data = data;
    }
}
