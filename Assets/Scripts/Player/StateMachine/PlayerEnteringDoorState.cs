using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerEnteringDoorState : PlayerMovementState
{
    private Door _door;
    private Vector3 _targetPosition;
    private const float _transitionTime = 1.2f;
    private const float _startMovingTime = 0.2f;
    private float _timer = 0f;
    private bool _isMoving = false;
    public PlayerEnteringDoorState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        reusableData.SpeedMultiplier = 1f;
        player.Collider.enabled = false;
        
        reusableData.MovementInput = Vector2.zero;
        
        _targetPosition.x = _door.transform.position.x - _door.transform.forward.x - player.transform.position.x;
        _targetPosition.z = _door.transform.position.z - _door.transform.forward.z - player.transform.position.z;
    }

    public override void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _startMovingTime && !_isMoving)
        {
            player.Animator.SetBool("running", true);
            reusableData.MovementInput.x = _targetPosition.x;
            reusableData.MovementInput.y = _targetPosition.z;
            _isMoving = true;
        }

        if (_timer < _transitionTime) return;
        
        stateMachine.ChangeState(stateMachine.IdlingState);
    }

    public override void HandleInput() { }

    public override void Exit()
    {
        player.Animator.SetBool("running", false);
        _timer = 0f;
        _door.GoToNextDoor();
        player.Collider.enabled = true;
        _isMoving = false;
    }

    public void SetDoor(Door door)
    {
        _door = door;
    }
}