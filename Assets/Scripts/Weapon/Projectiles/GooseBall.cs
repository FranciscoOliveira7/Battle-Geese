using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GooseBall : BasePorjectile
{
    private const float maxRecoilAngle = 10f;
    
    private Vector3 _direction;
    private Vector3 _velocity;

    [PunRPC]
    public override void Spawn(Vector3 position, Vector3 target, float speed, float damage, int layerMask)
    {
        base.Spawn(position, target, speed, damage, layerMask);
        
        Vector3 distance = target - position;
        
        _velocity = distance.normalized * speed;

        // Rotaciona o vetor
        Vector3 rotatedVector = Quaternion.AngleAxis(
            Random.Range(-maxRecoilAngle, maxRecoilAngle),
            Vector3.up) * _velocity;
        
        rigidbody.velocity = rotatedVector;
    }
}
