using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Room : MonoBehaviour
{
    public static Room activeRoom;

    public bool _startActive;
    [HideInInspector] public bool isInBattle = false;
    public event Action<bool> OnLock;
    private GameObject enemiesFolder;
    private EnemySpawner[] _enemySpawners;
    private bool isCleared = true;
    private PathGrid pathGrid;


    private void Start()
    {
        _enemySpawners = GetComponentsInChildren<EnemySpawner>();

        enemiesFolder = transform.Find("Enemies").gameObject;
        isCleared = enemiesFolder.transform.childCount == 0; // Set isCleared if there are no enemies
        pathGrid = FindObjectOfType<PathGrid>();
        if (pathGrid != null) pathGrid.OnGridGenerated += OnGridGenerated;
    }

    private void OnGridGenerated()
    {
        if (!_startActive) Deactivate();
        else activeRoom = this;
        pathGrid.OnGridGenerated -= OnGridGenerated;
    }

    public void OnEnemyDeath()
    {
        // If the room is not active, just do nothing
        if (!isInBattle) return;

        StartCoroutine(UpdateRoom());
    }

    IEnumerator UpdateRoom()
    {
        yield return null;

        if (CountEnemiesAlive() == 0) UnlockRoom();
    }

    [ContextMenu("Count enemies alive")]
    private int CountEnemiesAlive()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int enemiesCount = 0;

        foreach (GameObject enemy in enemies)
        {
            enemiesCount += enemy.activeInHierarchy ? 1 : 0;
        }


        return enemies.Length;
        // return enemies.transform.childCount;
    }

    private void UnlockRoom()
    {

        MusicManager.Instance.StopBattleMusic();
        isInBattle = false;
        isCleared = true;
        OnLock?.Invoke(false);
    }

    public void StartBattle()
    {
        if (isCleared || isInBattle) return;

        isInBattle = true;
        MusicManager.Instance.StartBattleMusic();

        OnLock?.Invoke(true);

        //StartCoroutine(SpawnEnemies());
        SpawnEnemies();
    }

    //private IEnumerator SpawnEnemies()
    //{
    //    foreach (var enemySpawner in _enemySpawners)
    //    {
    //        enemySpawner.Spawn();
    //        //yield return new WaitForSeconds(0.4f);
    //    }
    //}
    private void SpawnEnemies()
    {

        _enemySpawners[0].GenerateWave();

    }

    // I guess
    public void Activate() => gameObject.SetActive(true);
    public void Deactivate() => gameObject.SetActive(false);
}
