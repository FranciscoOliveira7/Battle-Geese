using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadScreenContr : MonoBehaviour
{
    public GameObject SingleP;
    public GameObject MultiP;

    private void Awake()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            MultiP.SetActive(true);
            SingleP.SetActive(false);
        }
        else
        {
            SingleP.SetActive(true);
            MultiP.SetActive(false);
        }
    }
}
