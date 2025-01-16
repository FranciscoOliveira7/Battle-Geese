using Unity.VisualScripting;
using UnityEngine;

public class CarotLineTask : Node
{
    private Transform _transform, _targetLastPos;
    private Transform _target;
    
    private LineRenderer lineRenderer;

    private float minLineLength = 8.0f; // Comprimento minimo da linha
    private LayerMask collisionLayer; // Camada para colisao do Raycast

    private float timer = 0f; // Temporizador para controlar o delay
    private bool lineDrawn = false;

    // Temporizador de destruir
    private float destroyTimer = 0f;
    private GameObject temporaryTarget;

    public CarotLineTask(Transform transform, Transform target, Transform targetLastPos)
    {
        _transform = transform;
        _target = target;
        _targetLastPos = targetLastPos;

        // Configurar camada de colisao
        collisionLayer = LayerMask.GetMask("Obstacle");
    }

    public override NodeState Evaluate()
    {
        timer += Time.deltaTime; // Incrementa o temporizador

        if (!lineDrawn && timer >= 0)
        {
            // Criar um novo LineRenderer para cada linha
            lineRenderer = CreateLineRenderer("LineRenderer");

            DrawLine(lineRenderer); // Passar o LineRenderer para desenhar a linha
            lineDrawn = true; // Marcar como desenhada
            timer = 0f; // Resetar o temporizador para controle do delay
            destroyTimer = 0.8f; // Definir o temporizador para destruir apos 1 segundo
        }

        // Verificar se o LineRenderer foi desenhado
        if (lineDrawn)
        {
            destroyTimer -= Time.deltaTime; // Subtrair o tempo para controle do delay de destruir

            if (destroyTimer <= 0f) // Quando o temporizador de destruir chegar a 0
            {
                if (lineRenderer != null) // Verificar se o LineRenderer ainda existe
                {
                    GameObject.Destroy(lineRenderer.gameObject); // Destruir o objeto LineRenderer
                }

                // Resetando as variaveis para permitir a criacao de uma nova linha
                lineDrawn = false;
                timer = 0f; // Resetando o temporizador para o proximo ciclo
                state = NodeState.SUCCESS; // Linha desenhada e destruida com sucesso
            }
            else
            {
                state = NodeState.RUNNING; // Linha ainda esta sendo exibida
            }
        }

        return state;
    }

    private LineRenderer CreateLineRenderer(string name)
    {
        GameObject lineRendererObject = new GameObject(name);
        LineRenderer line = lineRendererObject.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        return line;
    }

    private void DrawLine(LineRenderer line)
    {
        float currentDistance = Vector3.Distance(_transform.position, _target.position);

        // Verificar se a distancia e menor que o comprimento minimo
        if (currentDistance < minLineLength)
        {
            // Calcular dire��o entre o Sporetacus e o Player
            Vector3 direction = (_target.position - _transform.position).normalized;

            // Ajustar a posicao final para atender ao comprimento m�nimo
        }

        // Raycast para detectar colisao
        RaycastHit hit;
        if (Physics.Raycast(_transform.position, (_target.position - _transform.position).normalized, out hit, Vector3.Distance(_transform.position, _target.position), collisionLayer))
        {
            // Ajustar posicao final para o ponto de colisao
        }

        line.SetPosition(0, _transform.position);
        line.SetPosition(1, _target.position);
        
        _targetLastPos.position = _target.position;
    }
}