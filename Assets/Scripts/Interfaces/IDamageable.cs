using Photon.Pun;
using UnityEngine;

public interface IDamageable
{
    // [PunRPC]
    void Damage(float amount, Vector3 direction, HealthComponent source);
}

public static class DamageUtil
{
    // 20 is 20...
    private static Collider[] _colliders = new Collider[20];

    public static void DamageInRadius(Vector3 position,
        float radius,
        float damage,
        LayerMask layerMask
        )
    {
        var results = Physics.OverlapSphereNonAlloc(position, radius, _colliders, layerMask);

        if (results == 0) return;

        for (int i = 0; i < results; i++)
        {
            if (_colliders[i].TryGetComponent(out IDamageable damageable))
            {
                if (PhotonNetwork.IsConnectedAndReady)
                {
                    _colliders[i].GetComponent<PhotonView>().RPC("Damage",
                        RpcTarget.All,
                        damage, _colliders[i].transform.position - position, null);
                }
                else damageable.Damage(damage, _colliders[i].transform.position - position, null);
            }
        }
    }
}
