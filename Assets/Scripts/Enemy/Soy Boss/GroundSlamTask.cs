using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using BehaviourTree;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class GroundSlamTask : Node
{
    private GameObject _shockWave;
    private float originY;
    private Transform _transform;
    private float _timer;
    private float _v0;
    private BlobShadow _shadow;
    private AudioClip _jumpAudio;
    private Animator _animator;

    public GroundSlamTask(Transform transform, float v0, BlobShadow shadow, GameObject shockWave, AudioClip jumpAudio, Animator animator)
    {
        _v0 = v0;
        _transform = transform;
        originY = _transform.position.y;
        _shadow = shadow;
        _shockWave = shockWave;
        _jumpAudio = jumpAudio;
        _animator = animator;
    }
    
    public override NodeState Evaluate()
    {
        if (_timer == 0f) Enter();
        
        _timer += Time.deltaTime;

        _animator.SetBool("isJumping", true);

        float y = originY + _v0 * _timer - 5f * _timer * _timer;
        
        _transform.position = new(_transform.position.x, y, _transform.position.z);
        
        if (_transform.position.y < originY)
        {
            CreateShockWave();
            Reset();
            return NodeState.SUCCESS;
        }
        
        state = NodeState.RUNNING;
        return state;
    }

    private void CreateShockWave()
    {
        GameObject.Instantiate(_shockWave, _transform.position, Quaternion.identity);
    }

    private void Enter()
    {
        SoundManager.instance.PlayClip(_jumpAudio, _transform, 0.2f);
        _shadow.Transform.localScale = 9f * Vector3.one;
    }

    private void Reset()
    {
        _animator.SetBool("isJumping", false);
        _timer = 0;
        _transform.position = new(_transform.position.x, originY, _transform.position.z);
        _shadow.Transform.localScale = Vector3.zero;
    }
}
