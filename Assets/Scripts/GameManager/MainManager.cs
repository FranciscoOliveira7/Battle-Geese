using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public AllWeaponsList AllWeaponsList;

    public List<Player> players;
    public Player myPlayer;
    
    [SerializeField] private SpawnPlayer spawnPlayer;

    private Animator sceneTransition;

    [HideInInspector] public int deadPlayerCount;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        players = new();
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        myPlayer = player;
        
        sceneTransition = GameObject.FindGameObjectWithTag("HUD").transform.Find("Mask").GetComponent<Animator>();
    }

    private void Start()
    {
        if (PhotonNetwork.InRoom) SpawnPlayer();
    }

    // TODO: use this
    public void ChangeLevel(string levelName)
    {
        StartCoroutine(LevelTransition(levelName));
    }

    private IEnumerator LevelTransition(string levelName)
    {
        myPlayer.Input.DisableAction(myPlayer.Input.PlayerActions.Move, 10f);
        myPlayer.FlipSprite(false);
        sceneTransition.SetTrigger("start");
        
        yield return new WaitForSeconds(1f);
        
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PhotonNetwork.IsMasterClient)
                Server.Instance.ChangeScene(levelName);
        }
        else SceneManager.LoadScene(levelName);
    }

    public void SavePlayerWeapons()
    {
         PlayerData.weapon1 = myPlayer.Weapons[0].Data;
         PlayerData.weapon2 = myPlayer.Weapons[1].Data;
         PlayerData.hat = myPlayer.GetHat();
    }

    public void TransitionPlayerToNextRoom(Vector3 doorPosition)
    {
        foreach (Player player in players)
        {
            player.photonView.RPC("DoorTransition", RpcTarget.Others, doorPosition);
        }
    }
    
    public bool AllPlayersDead()
    {
        return deadPlayerCount == players.Count;
    }

    public Player ClosestPlayer(Vector3 target)
    {
        Player closestPlayer = myPlayer;
        
        foreach (Player player in players)
        {
            if (Vector3.Distance(player.transform.position, target) <
                Vector3.Distance(closestPlayer.transform.position, target))
            {
                closestPlayer = player;
            }
        }
        
        return closestPlayer;
    }

    private void SpawnPlayer()
    {
        spawnPlayer.Spawn();
    }

    public void SpawnProjectile(GameObject prefab, Vector3 position, Vector3 target, float speed, float damage, int layerMask)
    {
        GameObject projectile;
        
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            projectile = Instantiate(prefab);

            projectile.GetComponent<IProjectile>().Spawn(
                position,
                target,
                speed,
                damage,
                layerMask
            );
        }
        else
        {
            projectile = Server.Instance.InstantiateGameObject(Path.Combine("Projectiles", prefab.name), transform.position, Quaternion.identity);

            PhotonView.Get(projectile).RPC("Spawn",
                RpcTarget.All,
                position,
                target,
                speed,
                damage,
                layerMask
            );
        }
    }

    public void OnAllPlayersDead()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(AutoChangeLevel());
        }
    }

    IEnumerator AutoChangeLevel()
    {
        yield return new WaitForSeconds(1.5f);
        ChangeLevel("Lobby");
    }
}
