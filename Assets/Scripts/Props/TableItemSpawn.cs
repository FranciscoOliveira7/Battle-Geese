using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[System.Serializable]
public class ItemPrefab
{
    public GameObject prefab;
    public float spawnChance; // Chance to spawn this item (0 to 1)
    public bool guaranteeSpawn; // If true, this item is guaranteed to spawn
}



public class TableItemSpawn : MonoBehaviour
{
    [SerializeField] private List<ItemPrefab> itemPrefabs; // List of item prefabs with their spawn chances
    [SerializeField] private float overallSpawnChance = 0.5f; // Overall chance to spawn an item (0 to 1)

    private void Start()
    {
        SpawnItem();
    }

    private void SpawnItem()
    {


        foreach (var item in itemPrefabs)
        {
            if (item.guaranteeSpawn)
            {
                Instantiate(item.prefab, transform.position + Vector3.up, Quaternion.identity, transform);
                return; // Exit after spawning the guaranteed item
            }
        }
        // Determine if an item should be spawned based on the overall spawn chance
        if (Random.value <= overallSpawnChance)
        {
            // Check for guaranteed spawn items first


            // Calculate the total spawn chance for non-guaranteed items
            float totalSpawnChance = 0f;
            foreach (var item in itemPrefabs)
            {
                if (!item.guaranteeSpawn)
                {
                    totalSpawnChance += item.spawnChance;
                }
            }

            // Generate a random number between 0 and the total spawn chance
            float randomValue = Random.Range(0f, totalSpawnChance);

            // Iterate through the list and spawn an item based on its normalized spawn chance
            float cumulativeChance = 0f;
            foreach (var item in itemPrefabs)
            {
                if (!item.guaranteeSpawn)
                {
                    cumulativeChance += item.spawnChance;
                    if (randomValue <= cumulativeChance)
                    {
                        // Instantiate the selected item on top of the object
                        if (PhotonNetwork.IsConnectedAndReady)
                        {
                            if (!PhotonNetwork.IsMasterClient) return;
                            Server.Instance.InstantiateGameObject("WeaponItems/" + item.prefab.name, transform.position + Vector3.up, Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(item.prefab, transform.position + Vector3.up, Quaternion.identity);
                        }
                        break; // Exit the loop after spawning an item
                    }
                }
            }
        }
    }
}





