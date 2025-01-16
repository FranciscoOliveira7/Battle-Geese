using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrothersController : MonoBehaviour
{
    public OtoutoBt otoutoBt1;
    public OtoutoBt otoutoBt2;
    public Transform avoidant;
    public Animator animator1;
    public Animator animator2;

    public float timeToSwitch = 5f;

    private enum Phase { None, Aggressive, Avoiding }


    private void Start()
    {
        otoutoBt1 = transform.Find("Otouto").GetComponent<OtoutoBt>();
        otoutoBt2 = transform.Find("Ani").GetComponent<OtoutoBt>();
        animator1 = transform.Find("Otouto/Sprite").GetComponent<Animator>();
        animator2 = transform.Find("Ani/Sprite").GetComponent<Animator>();
        otoutoBt1.isAgresive = true;
        otoutoBt2.isAgresive = false;
        StartCoroutine(SwitchAgressive());
    }

    private IEnumerator SwitchAgressive()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToSwitch);
            otoutoBt1.isAgresive = !otoutoBt1.isAgresive;
            otoutoBt2.isAgresive = !otoutoBt2.isAgresive;

            if (otoutoBt1.isAgresive)
                animator1.SetBool("isAngry", true);
            else
                animator1.SetBool("isAngry", false);

            if (otoutoBt2.isAgresive)
                animator2.SetBool("isAngry", true);
            else
                animator2.SetBool("isAngry", false);

            // Trigger dash on phase change
            if (otoutoBt1.isAgresive)
            {
                ChangePhase(otoutoBt1, Phase.Aggressive);
                ChangePhase(otoutoBt2, Phase.Avoiding);
            }
            else
            {
                ChangePhase(otoutoBt1, Phase.Avoiding);
                ChangePhase(otoutoBt2, Phase.Aggressive);
            }
        }
    }

    private void ChangePhase(OtoutoBt otoutoBt, Phase newPhase)
    {
        if (newPhase == Phase.Avoiding)
        {
            otoutoBt._unit.movingTarget = avoidant;
            otoutoBt.hasDashes = true;

        }
        else
        {
            otoutoBt._unit.movingTarget = MainManager.Instance.ClosestPlayer(transform.position).transform;
            otoutoBt.hasDashes = true;
        }

    }
}
