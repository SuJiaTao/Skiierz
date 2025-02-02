using System;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SlopeTestsA"); // Replace with actual scene name
    }

    public void QuitGame()
    {
        Application.Quit(); 
        Debug.Log("Game is exiting");
    }

    public void LoadLevelSelect() {
        Debug.Log("level select pressed");
    }
}

