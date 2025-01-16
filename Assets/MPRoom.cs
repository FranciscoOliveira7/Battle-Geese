using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MPRoom : MonoBehaviour
{
    public TMP_Text Name;

    public void JoinRoom()
    {
        Server.Instance.JoinRoom(Name.text);
        // GameObject.Find("CreateAndJoin").GetComponent<CreateAndJoin>().JoinRoomInList(Name.text);
    }
}
