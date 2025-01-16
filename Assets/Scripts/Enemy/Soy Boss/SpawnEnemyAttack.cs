using System.Collections;
using System.Collections.Generic;
using System.IO;
using BehaviourTree;
using Photon.Pun;
using UnityEngine;

public class SpawnEnemyAttack : Node
{
    private Transform _transform;
    private GameObject[] _enemies;
    Collider[] _collider = new Collider[10];

    public SpawnEnemyAttack(Transform transformm, GameObject[] enemies)
    {
        _transform = transformm;
        _enemies = enemies;
    }
    
    public override NodeState Evaluate()
    {
        Vector3 spawnPos;
        int collisions = 0;
        do
        {
            Vector3 spawnOffset = Random.Range(-2.0f, 2.0f) * _transform.forward + Random.Range(-2.0f, 2.0f) * _transform.forward;
            spawnPos = _transform.position + spawnOffset;
            
            collisions = Physics.OverlapSphereNonAlloc(spawnPos, 0.25f, _collider, 1<<6);
        } while (collisions != 0);
        
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PhotonNetwork.IsMasterClient)
                Server.Instance.InstantiateGameObject(Path.Combine("Enemies", _enemies[Random.Range(0, _enemies.Length)].name), spawnPos, Quaternion.identity);
        }
        else GameObject.Instantiate(_enemies[Random.Range(0, _enemies.Length)], spawnPos, Quaternion.identity);

        state = NodeState.SUCCESS;
        return state;
    }
}
