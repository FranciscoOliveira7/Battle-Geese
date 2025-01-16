using BehaviourTree;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckPlayerInAttackRange : Node
{
    private Transform _transform;
    private Animator _animator;
    private float _attackRange;
    private Unit _unit;
    private string _attackType;
    private HealthComponent _health;

    private Transform canvasTransform;
    private Transform shieldTransform;

    public CheckPlayerInAttackRange(Transform transform, float attackRange, Unit unit, string attackType)
    {
        _transform = transform;
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _attackRange = attackRange;
        _unit = unit;
        _attackType = attackType;
    }
    public CheckPlayerInAttackRange(Transform transform, float attackRange, Unit unit, string attackType, HealthComponent healthComponent)
    {
        _transform = transform;
        _animator = transform.Find("Sprite").GetComponent<Animator>();

        _attackRange = attackRange;
        _unit = unit;
        _attackType = attackType;
        _health = healthComponent;

        canvasTransform = transform.Find("Canvas");
        shieldTransform = canvasTransform.Find("Shield");
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            Debug.Log("Target is null");
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;
        if (Vector3.Distance(_transform.position, target.position) <= _attackRange && _attackType != "dash")
        {
            _animator.SetBool("isWalking", false);
            shieldTransform.gameObject.SetActive(false);
            _health.dmgMultiplier = 1f;
            //Debug.Log(_health.dmgMultiplier);


            // _unit.isStopped = true;
            state = NodeState.SUCCESS;
            return state;
        }
        else if (_attackType != "dash")
        {
            _unit.isStopped = false;
            state = NodeState.FAILURE;
            return state;

        }
        else if (Vector3.Distance(_transform.position, target.position) <= _attackRange)
        {
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.FAILURE;
        return state;
    }

}