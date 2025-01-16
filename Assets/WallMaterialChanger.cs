using UnityEngine;
using UnityEngine.SceneManagement;

public class WallMaterialChanger : MonoBehaviour
{
    public Material LobbyMaterial;
    public Material Lvl1Material;
    public Material Lvl2Material;
    public Material Lvl3Material;
    // Add more materials as needed for other scenes

    private Renderer wallRenderer;

    void Start()
    {
        wallRenderer = GetComponent<Renderer>();
        ChangeMaterialBasedOnScene();
    }

    void ChangeMaterialBasedOnScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Lobby":
                wallRenderer.material = LobbyMaterial;
                break;
            case "Level_1":
                wallRenderer.material = Lvl1Material;
                break;
            case "Level_2":
                wallRenderer.material = Lvl2Material;
                break;
            case "Level_3":
                wallRenderer.material = Lvl3Material;
                break;
            // Add more cases for other scenes
            default:
                wallRenderer.material = LobbyMaterial;
                break;
        }
    }
}
