using System.Collections;
using UnityEngine;
using BehaviourTree;

public class TaskAttack : Node
{
    private Animator _animator;
    private Transform _lastTarget;
    private HealthComponent _playerHealthComponent;
    private Unit _unit;
    private Transform _position;

    private float _attackTime = 1f;
    private float _attackCounter = 0f;
    private float _cooldownCounter = 0f;
    private bool playerIsDead;
    private Vector3 _dashDirection;
    private string _attackType;
    private float _attackCooldown;

    private Weapon _weapon;

    private bool _isDashing;  // Controle para impedir repeti��o do dash

    private AnimationEventHandler _eventHandler;



    public TaskAttack(Transform transform, Unit unit, string attackType, float attackCooldown)
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _position = transform;
        _unit = unit;

        _attackType = attackType;
        _attackCooldown = attackCooldown;

        _weapon = transform.Find("Weapon").GetComponent<Weapon>();
        _weapon.Equip();

        _eventHandler = transform.Find("Sprite").GetComponent<AnimationEventHandler>();

        if (attackType == "spore")
        {
            _eventHandler.OnAttack += SporeBash;
            _eventHandler.OnFinish += SporePoke;

        }
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if (target != _lastTarget)
        {
            _playerHealthComponent = target.GetComponent<HealthComponent>();
            _lastTarget = target;
        }

        _attackCounter += Time.deltaTime;
        _cooldownCounter += Time.deltaTime;

        //Debug.Log("attackra" + _attackRange);
        //Debug.Log("bruh" + Vector3.Distance(_position.position, target.position));
        //if (_attackRange <= Vector3.Distance(_position.position, target.position))
        //{
        //    Debug.Log("ni");
        _unit.movingTarget = target;

        //}



        if (_attackCounter >= _attackTime)
        {
            if (_attackType == "dash" && !_isDashing)
            {
                if (_cooldownCounter >= _attackCooldown)
                {

                    _isDashing = true;
                    _position.GetComponent<MonoBehaviour>().StartCoroutine(DashTowardsTarget(target, 2f, 0.7f));
                }
            }
            else if (_attackType == "normal")
            {
                // playerBehaviour.Damage(10);

                _weapon.Direction = target.position - _position.position;
                //_animator.SetBool("isWalking", false);
                _animator.SetTrigger("Shoot");
                _weapon.Enter();
            }
            else if (_attackType == "spore")
            {
                int randomPoint = Random.Range(0, 100);

                if (randomPoint < 30)
                {
                    _animator.SetTrigger("Bash");
                }

                if (randomPoint >= 30)
                {
                    _animator.SetTrigger("Poke");
                }
            }
            else if (_attackType == "carot")
            {
                _weapon.Direction = target.position - _position.position;
                _animator.SetTrigger("Prep");
                _weapon.Enter();
                _animator.SetTrigger("Shoot");
            }

            if (_playerHealthComponent.Health < 0)
            {
                playerIsDead = true;
            }
            if (playerIsDead)
            {
                ClearData("target");
                _animator.SetBool("isWalking", true);
            }
            else
            {
                _attackCounter = 0f;
            }
        }

        state = NodeState.SUCCESS;
        return state;
    }


    private IEnumerator DashTowardsTarget(Transform target, float dashDistance, float duration)
    {
        _unit.isStopped = true;
        _animator.SetBool("isPrepping", true);

        // Pause before the dash
        yield return new WaitForSeconds(1.5f);

        _animator.SetBool("isPrepping", false);
        _animator.SetBool("isDashing", true);

        // Calculate dash direction and destination
        _dashDirection = (target.position - _position.position).normalized * dashDistance;
        _dashDirection.y = 0;

        float elapsedTime = 0f;
        bool hasProcd = false;
        // Perform the dash gradually
        _unit.rb.AddForce(_unit.speed * 2 * _dashDirection, ForceMode.VelocityChange);
        while (elapsedTime < duration)
        {
            if (_unit.rb.velocity.magnitude > 0.1f)
            {
                _unit.rb.AddForce(-_unit.rb.velocity, ForceMode.Acceleration);
            }
            
            float collisionRadius = 0.5f;
            float distance = Vector3.Distance(_position.position, target.position);

            if (distance <= collisionRadius && !hasProcd)
            {
                _playerHealthComponent.Damage(15, (target.position - _position.position).normalized, null); // Apply dash attack damage
                hasProcd = true;
            }

            // _position.Translate(_dashDirection * (_unit.speed * Time.deltaTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Finish the dash and set the target
        _unit.isStopped = false;
        _unit.movingTarget = target;

        _animator.SetBool("isDashing", false);
        _isDashing = false;  // Allow dash to be called again in a future evaluation
        _cooldownCounter = 0f;
    }

    private void SporeBash()
    {
        _playerHealthComponent.Damage(5, (_playerHealthComponent.transform.position - _position.position).normalized, null);
    }

    private void SporePoke()
    {
        _playerHealthComponent.Damage(15, (_playerHealthComponent.transform.position - _position.position).normalized, null);
    }
}
