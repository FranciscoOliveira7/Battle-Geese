using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BildBoardDirectionController : MonoBehaviour
{
    [SerializeField] private Sprite spritesLeft;
    [SerializeField] private Sprite spritesMiddle;
    [SerializeField] private Sprite spritesRight;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float difference = transform.position.x - Camera.main.transform.position.x;

        if (difference < -2f)
        {
            _spriteRenderer.sprite = spritesLeft;
        }
        else if (difference > 2f)
        {
            _spriteRenderer.sprite = spritesRight;
        }
        else
        {
            _spriteRenderer.sprite = spritesMiddle;
        }
    }
}
