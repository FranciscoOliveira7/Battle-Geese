using Photon.Pun;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject offlinePlayer;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float spawnOffsetX;
    private Vector3 spawnPos;

    internal void Spawn()
    {
        if (!PhotonNetwork.IsConnectedAndReady) return;
        if (offlinePlayer != null)
        {
            spawnPos = offlinePlayer.transform.position;
            Destroy(offlinePlayer);
        }
        
        Vector3 pos = spawnPos + Vector3.right * Random.Range(-spawnOffsetX, spawnOffsetX);

        Server.Instance.InstantiateGameObject(playerPrefab.name, pos, Quaternion.identity);
    }
}