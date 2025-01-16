using BehaviourTree;

using UnityEngine;

public class TaskGoToTarget : Node
{
    private Transform _transform;
    private Animator _animator;
    private Unit _unit;
    private HealthComponent _health;

    private Transform canvasTransform;
    private Transform shieldTransform;

    public TaskGoToTarget(Transform transform, Unit unit, HealthComponent health)
    {
        _transform = transform;
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _unit = unit;
        _health = health;

        canvasTransform = transform.Find("Canvas");
        shieldTransform = canvasTransform.Find("Shield");
    }
    public TaskGoToTarget(Transform transform, Unit unit)
    {
        _transform = transform;
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _unit = unit;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        // Verifica se existe um alvo e se a distância é maior que um valor mínimo
        if (target != null && Vector3.Distance(_transform.position, target.position) > 0.1f)
        {
            // Define o destino diretamente para o target.position
            _unit.movingTarget = target;

            // Atualiza as animações

            shieldTransform.gameObject.SetActive(true);
            _health.dmgMultiplier = 0.5f;
            _animator.SetBool("isWalking", true);
            state = NodeState.SUCCESS;

        }
        //else
        //{
        //    Debug.Log(false);
        //    _health.dmgMultiplier = 1f;
        //    _animator.SetBool("isWalking", false); // Para a animação de caminhada quando est� próximo
        //    state = NodeState.SUCCESS;
        //}

        return state;
    }

}
