using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject loadingScreen;
    
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        loadingScreen.SetActive(false);
    }
}
