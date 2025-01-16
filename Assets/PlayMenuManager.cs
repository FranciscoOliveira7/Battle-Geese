using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayMenuManager : MonoBehaviour
{
    public bool isPlayPanelOpen;
    public GameObject playPanel;

    // Referência ao VideoPlayer
    public VideoPlayer videoPlayer;
    public GameObject Intro;
    public RawImage MainImage;
    public Image Black;

    private void Start()
    {
        isPlayPanelOpen = false;

        // Inscreva-se no evento loopPointReached
        if (videoPlayer != null)
        {

            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPlayPanelOpen)
        {
            ClosePlayPanel();
        }
    }

    private void ClosePlayPanel()
    {
        playPanel.SetActive(false);
        isPlayPanelOpen = false;
    }

    public void PlaySingleplayer()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }

    public void PlayMultiplayer()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MP_LoadingScreen");
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Desativa o objeto que contém o VideoPlayer
        vp.gameObject.SetActive(false);
        Intro.SetActive(false);
        Black.gameObject.SetActive(false);
        MainImage.gameObject.SetActive(true);
    }
}
