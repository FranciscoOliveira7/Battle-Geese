using System;
using UnityEngine;
using BehaviourTree;
using UnityEngine.UIElements;

public class AimSpear : Node
{

    private Transform player;
    private Transform position;
    private Transform spear; // Referência ao objeto aninhado

    public AimSpear(Transform transform)
    {
        position = transform;

        // Substitua pelo caminho correto para encontrar o objeto filho aninhado
        spear = position.Find("Sprite/Spear");
        if (spear == null)
        {
            Debug.LogError("Spear not found!");
        }
    }

    public override NodeState Evaluate()
    {
        // Obtém o alvo a partir dos dados do comportamento
        player = (Transform)GetData("target");

        // Verifica se o alvo foi encontrado e se o `child` aninhado é válido
        if (player == null || spear == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        // Calcula a direção em relação ao jogador
        Vector2 directionToTarget = (player.position - position.position).normalized;
        float angle = Vector2.SignedAngle(Vector2.right, directionToTarget);  //change this too

        spear.eulerAngles = new Vector3(0, 0, angle);


        if (angle < -180)
        {
            angle += 360;
        }
        else if (angle > 180)
        {
            angle -= 360;
        }

        // Aplica a rotação ao `child` aninhado no eixo Z
        spear.localRotation = Quaternion.Euler(0, 0, angle);

        

        state = NodeState.RUNNING;
        return state;
    }
}
