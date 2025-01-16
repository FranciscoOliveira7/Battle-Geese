using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Tree = BehaviourTree.Tree;

public class SoyBossBT : Tree
{
    [Header("SoyRain")]
    [SerializeField] float _duration;
    [SerializeField] float _shootDelay;
    [SerializeField] float _shootSpeed;
    [SerializeField] float _damage;
    [Header("GroundSlam")]
    [SerializeField] GameObject _shockWave;
    [SerializeField] AudioClip _jumpSound;
    [SerializeField] float _groundSlamImpulse = 5f;
    [SerializeField] Animator _animator;

    [SerializeField] GameObject[] _minions;
    
    private BlobShadow _shadow;
    
    protected override Node SetupTree()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _shadow = GetComponent<BlobShadow>();
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new WaitTask(1.5f),
                new SoyRainAttack(_shootDelay, _shootSpeed, _damage, _duration, _animator),
                new SpawnEnemyAttack(transform, _minions),
                new SoyRainAttack(_shootDelay, _shootSpeed, _damage, _duration, _animator),
                new WaitTask(1f),
                new GroundSlamTask(transform, _groundSlamImpulse, _shadow, _shockWave, _jumpSound, _animator),
                new WaitTask(1f),
                new SpawnEnemyAttack(transform, _minions),
            })
        });

        return root;
    }
}
