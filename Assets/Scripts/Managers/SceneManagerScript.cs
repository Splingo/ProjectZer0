using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenuScene":
                Time.timeScale = 0.0f; break;
            case "CityScene":
                Time.timeScale = 1.0f; break;
            case "GameOverScene":
                Time.timeScale = 0.0f; break;
            default:
                break;
        }
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Function SceneManagerScript.Quit() has been called.");
        Application.Quit();
    }

}
