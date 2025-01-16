using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class TripleProjectile : BasePorjectile
{
    public GameObject projectilePrefab;
    private const float spread = 10f;
    
    // [PunRPC]
    // public void Spawn(Vector3 position, Vector3 target, float speed, float damage, LayerMask layerMask, HealthComponent sender)
    // {
    //     GameObject left;
    //     GameObject middle;
    //     GameObject right;
    //
    //     if (!PhotonNetwork.InRoom)
    //     {
    //         left = Instantiate(projectilePrefab, position, Quaternion.identity);
    //         middle = Instantiate(projectilePrefab, position, Quaternion.identity);
    //         right = Instantiate(projectilePrefab, position, Quaternion.identity);
    //     }
    //     else
    //     {
    //         left = Server.Instance.InstantiateGameObject(Path.Combine("Projectiles", projectilePrefab.name), transform.position, Quaternion.identity);
    //         middle = Server.Instance.InstantiateGameObject(Path.Combine("Projectiles", projectilePrefab.name), transform.position, Quaternion.identity);
    //         right = Server.Instance.InstantiateGameObject(Path.Combine("Projectiles", projectilePrefab.name), transform.position, Quaternion.identity);
    //     }
    //
    //     // Rotaciona o vetor
    //     Vector3 leftTarget = (Quaternion.AngleAxis(-spread, Vector3.up) * (target - position)) + position;
    //     Vector3 rightTarget = (Quaternion.AngleAxis(spread, Vector3.up) * (target - position)) + position;
    //     
    //     left.GetComponent<IProjectile>().Spawn(
    //         position,
    //         leftTarget,
    //         speed,
    //         damage,
    //         layerMask,
    //         sender
    //     );
    //     middle.GetComponent<IProjectile>().Spawn(
    //         position,
    //         target,
    //         speed,
    //         damage,
    //         layerMask,
    //         sender
    //     );
    //     right.GetComponent<IProjectile>().Spawn(
    //         position,
    //         rightTarget,
    //         speed,
    //         damage,
    //         layerMask,
    //         sender
    //     );
    // }
    
    [PunRPC]
    public override void Spawn(Vector3 position, Vector3 target, float speed, float damage, int layerMask)
    {
        // Rotaciona o vetor
        Vector3 leftTarget = (Quaternion.AngleAxis(-spread, Vector3.up) * (target - position)) + position;
        Vector3 rightTarget = (Quaternion.AngleAxis(spread, Vector3.up) * (target - position)) + position;

        if (PhotonNetwork.IsConnected && !PhotonView.Get(this).IsMine) return;
        
        MainManager.Instance.SpawnProjectile(
            projectilePrefab,
            position,
            leftTarget,
            speed,
            damage,
            layerMask
        );
        MainManager.Instance.SpawnProjectile(
            projectilePrefab,
            position,
            target,
            speed,
            damage,
            layerMask
        );
        MainManager.Instance.SpawnProjectile(
            projectilePrefab,
            position,
            rightTarget,
            speed,
            damage,
            layerMask
        );
    }
}
