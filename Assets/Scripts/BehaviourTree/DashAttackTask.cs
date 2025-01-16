using BehaviourTree;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class DashAttackTask : Node
{
    private Ray _ray;
    private Unit _unit { get; }
    private float _prepDuration { get; }
    private float _duration { get; }
    private float _timeElapsed;
    private float _speed { get; }
    private Animator _animator { get; }
    private bool _isPrepping = true;
    private Vector3 _direction;

    public event Action OnDashCompleted; // Event to notify when the dash is completed

    public DashAttackTask(Unit unit, float duration, float prepDuration, float speed, Animator animator)
    {
        _unit = unit;
        _duration = duration;
        _prepDuration = prepDuration;
        _timeElapsed = 0f;
        _speed = speed;
        _animator = animator;
    }

    public override NodeState Evaluate()
    {
        // Dash phase enter
        if (_timeElapsed == 0)
        {
            if (_isPrepping) Anticipate();
            else PerformDash();
        }

        // The rest is waiting...
        _timeElapsed += Time.deltaTime;

        if (_isPrepping)
        {
            if (_timeElapsed >= _prepDuration)
            {
                if (_isPrepping) _animator.SetBool("isPrepping", false);
                _isPrepping = false;
                _timeElapsed = 0f;
            }
        }
        else
        {
            if (_timeElapsed >= _duration)
            {
                ResetValues(); // for the next dash
                OnDashCompleted?.Invoke(); // Invoke the event when the dash is completed
                return NodeState.SUCCESS;
            }
        }

        return NodeState.RUNNING;
    }

    private void Anticipate()
    {
        _unit.isStopped = true;
        _animator.SetBool("isPrepping", true);
    }

    private void PerformDash()
    {
        _unit.isDecelerating = false;
        _direction = (_unit.movingTarget.position - _unit.transform.position).normalized;
        _animator.SetBool("isPrepping", false);
        _animator.SetBool("isDashing", true);
        _unit.rb.AddForce(_direction * _speed, ForceMode.VelocityChange);
    }

    private void ResetValues()
    {
        _isPrepping = true;
        _unit.isDecelerating = true;
        _unit.isStopped = false;
        _timeElapsed = 0f;
        _animator.SetBool("isDashing", false);
    }
}





