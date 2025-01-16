using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;

public class RoomList : MonoBehaviourPunCallbacks
{
    public GameObject roomPrefab;
    public GameObject[] rooms;
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i] != null)
            {
                Destroy(rooms[i]);
            }
        }
        
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].IsOpen && roomList[i].IsVisible && roomList[i].PlayerCount >= 1)
            {
                GameObject room = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Content").transform);
                room.GetComponent<MPRoom>().Name.text = roomList[i].Name;
            
                rooms[i] = room;
            }
        }
    }
}
