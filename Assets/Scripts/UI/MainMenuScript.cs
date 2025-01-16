using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public PlayMenuManager playMenuManager;


    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);

    }

    public void ShowOptionsMenu()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void PlayGame()
    {
        playMenuManager.isPlayPanelOpen = true;
        playMenuManager.playPanel.SetActive(true);
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
