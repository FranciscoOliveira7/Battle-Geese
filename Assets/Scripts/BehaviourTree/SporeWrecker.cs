using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeWrecker : MonoBehaviour
{
    private GameObject sporewall;
    private Animator animator;
    private float timer;
    // Start is called before the first frame update
    void OnDestroy()
    {
        sporewall = GameObject.Find("SporetacusRoom/SporeWall");

        if (sporewall != null)
        { 
            foreach (Transform child in sporewall.transform)
            {
                animator = child.GetComponent<Animator>();
                animator.SetBool("isStarted", false);
                animator.SetTrigger("gone");

            }

            timer += Time.deltaTime;

            if (timer > 0.5)
            {
                Destroy(sporewall);
            }
        }

    }
}
