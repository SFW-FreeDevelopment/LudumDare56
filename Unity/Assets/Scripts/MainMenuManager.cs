using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OnClickSettings()
    {
        // TODO: Open Settings window
    }

    public void OnClickCredits()
    {
        // TODO: Open Credits window
    }
}
