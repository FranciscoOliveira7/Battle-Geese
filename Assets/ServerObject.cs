using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

public class ServerObject : MonoBehaviour
{
    public string prefabName;

    private void Awake()
    {
        if (PhotonNetwork.IsConnected && PhotonView.Get(this).Owner == null)
        {
            Server.Instance.InstantiateGameObject(prefabName, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    // private void SetPrefabName(string name)
    // {
    //     this.prefabName = name;
    // }
    //
    // [DidReloadScripts]
    // private static void OnRecompile()
    // {
    //     string name = PrefabUtility.GetCorrespondingObjectFromSource(gameObject).name;
    //     this.SetPrefabName(name);
    // }
}