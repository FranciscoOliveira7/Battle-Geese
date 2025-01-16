using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public TMP_InputField input_Create;
    public TMP_InputField input_Join;
    
    [SerializeField] private GameObject MPUI;
    [SerializeField] private GameObject loadingScreen;
    
    public override void OnJoinedLobby()
    {
        MPUI.SetActive(true);
    }

    public void CreateRoom()
    {
        MPUI.SetActive(false);
        loadingScreen.SetActive(true);
        Server.Instance.CreateRoom(input_Create.text);
    }

    public void JoinRoom()
    {
        Server.Instance.JoinRoom(input_Join.text);
    }
}
