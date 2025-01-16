using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool OptionsMenuActive = false;

    public GameObject PauseMenuUI;
    public GameObject OptionsMenuUI;

    public AudioMixer audioMixer;


    void Start()
    {
        PauseMenuUI.SetActive(false);
        OptionsMenuUI.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused && OptionsMenuActive == false)
            {
                Resume();
            }
            else if (GameIsPaused && OptionsMenuActive)
            {
                OptionsMenuUI.SetActive(false);
                PauseMenuUI.SetActive(true);
                OptionsMenuActive = false;
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        //Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        //Time.timeScale = 0f;


        GameIsPaused = true;
    }

    public void Options()
    {

        OptionsMenuActive = true;
        PauseMenuUI.SetActive(false);
        OptionsMenuUI.SetActive(true);

    }

    public void SetVolumeMaster(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
    public void SetVolumeMusic(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetVolumeSFX(float volume)
    {
        audioMixer.SetFloat("SoundVolume", volume);
    }

    public void Quit()
    {
        //Time.timeScale = 1f;
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            SceneManager.LoadScene("MainMenuScene");
            return;
        }
        PhotonNetwork.LeaveRoom();
    }
}
