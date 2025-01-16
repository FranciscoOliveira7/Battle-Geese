using System.IO;
using Photon.Pun;
using UnityEngine;

public class ProjectileSpawner : WeaponComponent<ProjectileSpawnerData, AttackProjectileSpawner>
{
    private Hammo hammo;
    private void SpawnProjectile()
    {
        if (PhotonNetwork.IsConnectedAndReady && !PhotonView.Get(this).IsMine) return;
        
        MainManager.Instance.SpawnProjectile(
            data.AttackData.Projectile,
            transform.position,
            weapon.TargetPosition,
            data.Speed,
            data.Damage,
            data.DetectableLayers
        );
        // if (!PhotonNetwork.IsConnectedAndReady)
        // {
        //     GameObject projectile = Instantiate(data.AttackData.Projectile);
        //
        //     projectile.GetComponent<IProjectile>().Spawn(
        //         transform.position,
        //         weapon.TargetPosition,
        //         data.Speed,
        //         data.Damage,
        //         data.DetectableLayers
        //     );
        // }
        // else
        // {
        //     GameObject projectile = Server.Instance.InstantiateGameObject(Path.Combine("Projectiles", data.AttackData.Projectile.name), transform.position, Quaternion.identity);
        //
        //     PhotonView photonView = PhotonView.Get(projectile);
        //     
        //     photonView.RPC("Spawn",
        //         RpcTarget.All,
        //         transform.position,
        //         weapon.TargetPosition,
        //         data.Speed,
        //         data.Damage,
        //         (int)data.DetectableLayers
        //     );
        // }
    }

    protected override void Start()
    {
        base.Start();

        // Try to get the Hammo component if it exists
        hammo = GetComponent<Hammo>();

        EventHandler.OnAttack += HandleAttack;
    }

    private void HandleAttack()
    {
        if (hammo != null)
        {
            // Check if there is enough ammo
            if (hammo.CanUseHammo(1)) // Assuming each projectile uses 1 ammo
            {
                hammo.UseHammo(1);
                SpawnProjectile();
            }
        }
        else
        {
            // Fire projectile without checking for ammo
            SpawnProjectile();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventHandler.OnAttack -= HandleAttack;
    }
}