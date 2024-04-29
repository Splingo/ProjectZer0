using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Function QuitGame.Quit() has been called.");
        Application.Quit();
    }
}
