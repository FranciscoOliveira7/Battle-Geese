using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class skipintro : MonoBehaviour
{
    public GameObject[] gameObjectsIWantToDelete;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) BEGONE();
        if (Input.GetKey(KeyCode.Escape)) BEGONE();
        if (Input.GetKey(KeyCode.KeypadEnter)) BEGONE();
        if (Input.GetKey(KeyCode.Return)) BEGONE();
    }

    private void BEGONE()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.time = videoPlayer.clip.length;
    }
}
