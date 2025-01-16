using UnityEngine;

public class SporeLineTask : Node
{
    public LineRenderer lineRenderer;
    public GameObject first; // Player
    public GameObject second; // Sporetacus

    public float minLineLength = 8.0f; // Comprimento m�nimo da linha
    public LayerMask collisionLayer; // Camada para colis�o do Raycast

    private float timer = 0f; // Temporizador para controlar o delay
    private bool lineDrawn = false;

    // Temporizador de destrui��o
    private float destroyTimer = 0f;
    private GameObject temporaryTarget;
    public Transform targetTransform; // Transform do alvo calculado

    public Sporehit sporehit;
    public GameObject goonpos;

    //private Sporehit _sporehit;
    public SporeLineTask()
    {
        first = GameObject.FindWithTag("Player");
        second = GameObject.FindWithTag("Enemy");

        // Configurar camada de colis�o
        collisionLayer = LayerMask.GetMask("Obstacle");

        // Verificar se o Player e o Sporetacus existem
        if (first == null || second == null)
        {
            Debug.LogError("Player ou Sporetacus n�o encontrados na cena!");
        }


        goonpos = GameObject.Find("SporetacusRoom/Goonspos");
        sporehit = goonpos.GetComponent<Sporehit>();
        //_sporehit = sporehit;

        temporaryTarget = new GameObject("TemporaryTarget");
        targetTransform = temporaryTarget.transform;
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
            destroyTimer = 0.8f; // Definir o temporizador para destruir ap�s 1 segundo
        }

        // Verificar se o LineRenderer foi desenhado
        if (lineDrawn)
        {
            destroyTimer -= Time.deltaTime; // Subtrair o tempo para controle do delay de destrui��o

            if (destroyTimer <= 0f) // Quando o temporizador de destruir chegar a 0
            {
                if (lineRenderer != null) // Verificar se o LineRenderer ainda existe
                {
                    GameObject.Destroy(lineRenderer.gameObject); // Destruir o objeto LineRenderer
                }

                // Resetando as vari�veis para permitir a criacao de uma nova linha
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
        if (first == null || second == null)
        {
            Debug.LogError("First ou Second est� nulo. Certifique-se de que os objetos existem na cena.");
            return;
        }

        Vector3 sporetacusPosition = second.transform.position;

        // Atualizar a posi��o do Player (pegar a posi��o atual do Player)
        Vector3 playerPosition = first.transform.position;

        // Calcular a dist�ncia atual
        float currentDistance = Vector3.Distance(sporetacusPosition, playerPosition);

        // Verificar se a dist�ncia � menor que o comprimento m�nimo
        if (currentDistance < minLineLength)
        {
            // Calcular dire��o entre o Sporetacus e o Player
            Vector3 direction = (playerPosition - sporetacusPosition).normalized;

            // Ajustar a posi��o final para atender ao comprimento m�nimo
            playerPosition = sporetacusPosition + direction * minLineLength;
        }

        // Raycast para detectar colis�o
        RaycastHit hit;
        if (Physics.Raycast(sporetacusPosition, (playerPosition - sporetacusPosition).normalized, out hit, Vector3.Distance(sporetacusPosition, playerPosition), collisionLayer))
        {
            // Ajustar posi��o final para o ponto de colis�o
            playerPosition = hit.point;
        }

        targetTransform.position = playerPosition;

        line.SetPosition(0, sporetacusPosition);
        line.SetPosition(1, playerPosition);

        sporehit._throwSpot = targetTransform;
    }
}