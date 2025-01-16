using BehaviourTree;
using UnityEngine;

public class TaskCheckCollision : Node
{
    private Transform _transform;
    private HealthComponent _playerHealthComponent;
    private float timer;
    private float cooldown;

    public TaskCheckCollision(Transform transform)
    {
        _transform = transform;
        timer = 0;
        cooldown = 1.5f;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if (target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        _playerHealthComponent = target.GetComponent<HealthComponent>();

        if (_playerHealthComponent == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        float distance = Vector3.Distance(_transform.position, target.position);

        // Define the collision radius (adjust as needed)
        float collisionRadius = 0.5f;

        timer += Time.deltaTime;
        if (distance <= collisionRadius)
        {
            if (timer >= cooldown)
            {
                _playerHealthComponent.Damage(5, (_playerHealthComponent.transform.position - _transform.position).normalized, null);
                timer = 0;
            }
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
