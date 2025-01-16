using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.ProBuilder;

public class SporeGooning : MonoBehaviour
{
    private Animator animator;
    private float timer;
    private Weapon weapon;
    private int facing;

    void OnEnable()
    {

        animator = transform.GetComponent<Animator>();
        weapon = transform.Find("Weapon").GetComponent<Weapon>();
        animator.SetBool("isStart", true);

        facing = Random.Range(0, 2);

            if (facing == 0)
            {
                animator.SetBool("isLeft", true);
            }
            else
            {
                animator.SetBool("isRight", true);
            }



    }

    private void Update()
    {     
        timer += Time.deltaTime;

        if (timer > 1)
        {
            Attack(facing);
        }
        if (timer > 1.5)
        {
            animator.SetBool("isStart", false);
        }
        if (timer > 2)
        {
            animator.SetBool("isLeft", false);
            animator.SetBool("isRight", false);
            timer = 0;
            gameObject.SetActive(false);
        }
    }


    private void Attack(int face)
    {
        weapon = animator.transform.Find("Weapon").GetComponent<Weapon>();

        // Corrigir a escala e o sprite em um único método
        FlipWeapon(face);

        // Configurar e ativar o weapon
        weapon.Equip();
        weapon.Enter();
    }

    private void FlipWeapon(int face)
    {
        float direction = face == 0 ? -1 : 1;

        // Ajusta escala local
        weapon.transform.localScale = new Vector3(direction, 1, 1);

        // Alinha o sprite ao lado correto
        weapon.FlipWeaponSprite(face == 0 ? Facing.left : Facing.right);
    }

}
