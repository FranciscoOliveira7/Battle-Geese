using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusicStarter : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject funnymonkey;

    private void OnDisable()
    {
        Debug.Log(funnymonkey);
        funnymonkey.SetActive(true);
    }
}
