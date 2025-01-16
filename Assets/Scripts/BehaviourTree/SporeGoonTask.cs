using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UIElements.Experimental;

public class SporeGoonTask : Node
{
    private GameObject goonPos;
    private Animator _animator;

    private EnemyHealthComponent _healthComponent;
    private float SporeHealth;
    private float MaxSporeHealth;

    private int rand1;
    private int rand2;
    private int rand3;
    private int rand4;
    private int rand5;

    int current;

    public SporeGooning gooning;
    Weapon _weapon;

    public SporeGoonTask(EnemyHealthComponent healthComponent)
    {
        _healthComponent = healthComponent;
        SporeHealth = healthComponent.Health;
        MaxSporeHealth = healthComponent.MaxHealth;

        goonPos = GameObject.Find("SporetacusRoom/Goonspos");
        gooning = goonPos.GetComponent<SporeGooning>();


    }

     //first area: end at 10
     //second area: 11 - 17
     //third area: 18 - 22

    public override NodeState Evaluate()
    {
        SporeHealth = _healthComponent.Health;
        
        if (SporeHealth <= MaxSporeHealth && SporeHealth > 0.7)
        {
            rand1 = Random.Range(0, 22);
            rand2 = Random.Range(0, 22);
            rand3 = Random.Range(0, 22);
            rand4 = Random.Range(0, 22);
            rand5 = Random.Range(0, 22);
        }
        if (SporeHealth <= MaxSporeHealth * 0.7 && SporeHealth > MaxSporeHealth * 0.35)
        {
            rand1 = Random.Range(9, 22);
            rand2 = Random.Range(9, 22);
            rand3 = Random.Range(9, 22);
            rand4 = Random.Range(9, 22);
            rand5 = -1;
        }
        if (SporeHealth <= MaxSporeHealth * 0.35)
        {
            rand1 = Random.Range(18, 22);
            rand2 = Random.Range(18, 22);
            rand3 = Random.Range(18, 22);
            rand4 = -1;
            rand5 = -1;
        }

        current = 0;
        foreach (Transform child in goonPos.transform)
        {
           
            if (current == rand1 || current == rand2 || current == rand3 || current == rand4 || current == rand5)
            {
                SpawnGoon(child);
            }

            current++;
        }
     
        state = NodeState.SUCCESS;
        return state;
    }

    void SpawnGoon(Transform goon)
    {
        goon.gameObject.SetActive(true);

    }


    //void Goon()
    //{

    //    if (facing == 0)
    //    {
    //        _animator.SetBool("isLeft", true);
    //    }
    //    else
    //    {
    //        _animator.SetBool("isRight", true);
    //    }

    //    Attack(facing);

    //    _animator.SetBool("isStart", false);

    //}

    //private void Attack(int face)
    //{
    //    _weapon = _animator.transform.Find("Weapon").GetComponent<Weapon>();

    //    if (_weapon.transform.lossyScale == Vector3.one)
    //        _weapon.transform.localScale = new Vector3(face == 0 ? -1 : 1, 1, 1);

    //    _weapon.FlipWeaponSprite(face == 0 ? Facing.left : Facing.right);

    //    _weapon.Equip();

    //    _weapon.Enter();
    //}

}

