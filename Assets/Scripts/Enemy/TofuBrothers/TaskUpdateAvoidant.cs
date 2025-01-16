using BehaviourTree;
using UnityEngine;

public class TaskUpdateAvoidantPosition : Node
{
    private Transform _targetPos;
    private Transform _avoidant;
    private LayerMask _wallLayerMask;


    public TaskUpdateAvoidantPosition(Transform avoidant, LayerMask wallLayerMask, Transform target)
    {

        _avoidant = avoidant;
        _wallLayerMask = wallLayerMask;
        _targetPos = target;

    }

    public override NodeState Evaluate()
    {



        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;


        // Logic to update the position of the Avoidant
        Vector3 directionToAvoid = (_targetPos.transform.position - player.position).normalized;
        Vector3 newAvoidantPosition = _targetPos.transform.position + directionToAvoid * 2f; // Move 2 units away from the player

        // Check for walls and adjust the position
        if (Physics.Raycast(_targetPos.transform.position, directionToAvoid, out RaycastHit hit, 2f, _wallLayerMask))
        {
            // If a wall is detected, move the avoidant to the side
            Vector3 perpendicularDirection = Vector3.Cross(directionToAvoid, Vector3.up).normalized;
            newAvoidantPosition = _targetPos.transform.position + perpendicularDirection * 2f; // Move 2 units to the side
        }

        _avoidant.position = newAvoidantPosition;


        state = NodeState.SUCCESS;

        return state;
    }
}





