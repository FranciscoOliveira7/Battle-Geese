using System;
using System.Collections;
using UnityEngine;

public class EnemySpriteFlip : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private float _lastPosition;
    private Weapon[] weapons;
    private Transform sprite;
    

    private void Awake()
    {
        sprite = transform.Find("Sprite");
        _spriteRenderer = sprite.GetComponent<SpriteRenderer>();

        StartCoroutine(nameof(H));

        weapons = GetComponentsInChildren<Weapon>(); //yes, I know, it's not the best way to do

        // _currentWeapon.OnExit += ExitHandler;
    }

    // public EnemySpriteFlip(Transform transform)
    // {
    //     _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    //     
    //     // _currentWeapon.OnExit += ExitHandler;
    // }

    IEnumerator H()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            
            ActualFlip();
        }
    }

    private void ActualFlip()
    {
        if (Math.Abs(_lastPosition - transform.position.x) < 0.05f) return;
        
        ActuallyFlipFrThisTime();
        
        _lastPosition = transform.transform.position.x;
    }

    private void ActuallyFlipFrThisTime()
    {
        // _spriteRenderer.flipX = transform.transform.position.x - _lastPosition > 0f;
        
        Vector3 scale = sprite.localScale;
        scale.x = Math.Abs(scale.x);
        scale.x *= transform.transform.position.x - _lastPosition > 0f ? -1 : 1;
        sprite.localScale = scale;
        
        // if (weapons.Length > 0)
        // {
        //     foreach (var weapon in weapons)
        //     {
        //         weapon.Direction = _spriteRenderer.flipX ? Vector3.right : Vector3.left;
        //     }
        // }
    }

    public void ManuallyFlip(bool flipped)
    {
        Vector3 scale = sprite.localScale;;
        scale.x = Math.Abs(scale.x);
        scale.x *= flipped ? -1 : 1;
        sprite.localScale = scale;
    }
}
