using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class SoyRainAttack : Node
{
    private Transform _transform;
    private float _timer;
    private float _timer2;
    private float _timer3;
    private float _shootDelay;
    private float _speed;
    private float _damage;
    private float _duration;
    private SoySpawner _soySpawner;
    private Animator _animator;

    private float _bubbleDur;
    private float _boilDur;
    private float _openDur;

    public SoyRainAttack(float shootDelay, float shootSpeed, float damage, float duration, Animator animator)
    {
        _shootDelay = shootDelay;
        _speed = shootSpeed;
        _damage = damage;
        _duration = duration;
        _soySpawner = GameObject.Find("SoySpawner").GetComponent<SoySpawner>();
        _animator = animator;

        _bubbleDur = 1.6f;

        _timer = -1.6f;
        _timer2 = -1.6f;
        _timer3 = 0f;
    }

    public override NodeState Evaluate()
    {
        _timer += Time.deltaTime;
        _timer2 += Time.deltaTime;
        _timer3 += Time.deltaTime;

        Debug.Log(_animator.GetBool("isbubbling"));

        // time between start of task and first animation
        if (_timer3 < _bubbleDur)
        {
            _animator.SetBool("isbubbling", true);
        }

        // time between first animation and second animation
        if (_bubbleDur < _timer3)
        {
            _animator.SetBool("isbubbling", false);
            _animator.SetBool("isboiling", true);
        }


        // end of the task
        if (_timer2 > _duration)
        {
            _timer = -1.6f;
            _timer2 = -1.6f;
            _timer3 = 0f;

            // Ensure bubbling is off
            _animator.SetBool("isbubbling", false);
            _animator.SetBool("isboiling", false);

            return NodeState.SUCCESS;
        }

        // time between each shot
        if (_timer > _shootDelay)
        {
            _soySpawner.Spawn(_speed, _damage);
            _timer -= _shootDelay;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
