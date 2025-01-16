using System.Collections;
using System.Collections.Generic;
using System.Threading;
using BehaviourTree;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SporeGrowthTask : Node
{

    private GameObject SporeWall;
    private Animator _animatorSpore;
    private bool setup = false;
    private bool phase1 = false;
    private bool phase2 = false;
    private EnemyHealthComponent _healthComponent;
    private float SporeHealth;
    private float MaxSporeHealth;
 

    public SporeGrowthTask(EnemyHealthComponent healthComponent)
    {
        SporeWall = GameObject.Find("SporetacusRoom/SporeWall");
        _healthComponent = healthComponent;
        SporeHealth = healthComponent.Health;
        MaxSporeHealth = healthComponent.MaxHealth;
    }

    public override NodeState Evaluate()
    {
        if (!setup)
        { 
            foreach (Transform child in SporeWall.transform)
            {
                _animatorSpore = child.GetComponent<Animator>();
                _animatorSpore.SetBool("isStarted", true);
                // Obtenha o script de FacingScript de cada filho
                FacingScript facing = child.GetComponent<FacingScript>();

                // Verifica a direção e executa lógica específica
                if (facing.left)
                {
                    _animatorSpore.SetBool("isLeft", true);
                }
                else if (facing.right)
                {
                    _animatorSpore.SetBool("isRight", true);
                }
                else if (facing.up)
                {
                    _animatorSpore.SetBool("isUp", true);
                }
                else
                {
                    _animatorSpore.SetBool("isDown", true);
                }
            }
            setup = true;
        }
        SporeHealth = _healthComponent.Health;


        if (SporeHealth < MaxSporeHealth * 0.7)
        { 
            if (setup && !phase1)
            {

                GameObject sporedel1 = GameObject.Find("SporetacusRoom/SporeWall/Sporewall5");
                GameObject sporedel2 = GameObject.Find("SporetacusRoom/SporeWall/Sporewall15");
                Object.Destroy(sporedel1);
                Object.Destroy(sporedel2);
                SporeWall = GameObject.Find("SporetacusRoom/SporeWall");
                
                AlgumaCoisa();
                phase1 = true;
                
            }

        }

        if (SporeHealth < MaxSporeHealth * 0.35)
        {
            if (setup && !phase2)
            {
                GameObject sporedel3 = GameObject.Find("SporetacusRoom/SporeWall/Sporewall6");
                GameObject sporedel4 = GameObject.Find("SporetacusRoom/SporeWall/Sporewall14");
                GameObject sporedel5 = GameObject.Find("SporetacusRoom/SporeWall/Sporewall4");
                GameObject sporedel6 = GameObject.Find("SporetacusRoom/SporeWall/Sporewall21");
                Object.Destroy(sporedel3);
                Object.Destroy(sporedel4);
                Object.Destroy(sporedel5);
                Object.Destroy(sporedel6);

                SporeWall = GameObject.Find("SporetacusRoom/SporeWall");
                AlgumaCoisa();
                phase2 = true;

            }

        }


        state = NodeState.SUCCESS;
        return state;
    }
    
    private void AlgumaCoisa()
    {
        foreach (Transform child in SporeWall.transform)
        {
            Vector3 direction;
            FacingScript facing = child.GetComponent<FacingScript>();
            
            if (facing.left)       direction = new Vector3(-1f, 0f, 0f);
            else if (facing.right) direction = new Vector3( 1f, 0f, 0f);
            else if (facing.up)    direction = new Vector3( 0f, 0f, 1f);
            else                   direction = new Vector3( 0f, 0f,-1f);
            
            MainManager.Instance.StartCoroutine(LerpTranslation(child, direction, 1f));
        }
    }

    private IEnumerator LerpTranslation(Transform transform, Vector3 translation, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + translation;

        while (time < duration)
        {
            if (transform == null) yield break;
            transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }
}