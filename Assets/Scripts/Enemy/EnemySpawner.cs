using System.IO;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public int CurrentWave;
    public int WaveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public List<Transform> spawnLocations; // List of spawn locations
    public float WaveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;


    private void Awake()
    {

        spawnLocations = GetComponentsInChildren<Transform>().ToList();
        spawnLocations.Remove(transform);


    }
    void FixedUpdate()
    {
        if (spawnTimer <= 0)
        {
            if (enemiesToSpawn.Count > 0)
            {
                // Randomly select a spawn location from the list
                int randSpawnIndex = Random.Range(0, spawnLocations.Count);
                Transform selectedSpawnLocation = spawnLocations[randSpawnIndex];

                if (!PhotonNetwork.IsConnectedAndReady)
                    Instantiate(enemiesToSpawn[0], selectedSpawnLocation.position, Quaternion.identity);
                else if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.Instantiate(Path.Combine("Enemies", enemiesToSpawn[0].name), selectedSpawnLocation.position, Quaternion.identity);
                enemiesToSpawn.RemoveAt(0);
                spawnTimer = spawnInterval;
            }
            else
            {
                waveTimer = 0;
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
    }

    public void GenerateWave()
    {
        WaveValue = CurrentWave * 4;
        GenerateEnemies();

        spawnInterval = WaveDuration;/* / enemiesToSpawn.Count;*/
        waveTimer = WaveDuration;
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while (WaveValue > 0)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;
            if ((WaveValue - randEnemyCost) >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                WaveValue -= randEnemyCost;
            }
            else if (WaveValue < 0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}
