using System;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuController : MonoBehaviour
{
    public Camera mainCamera;
    public Animator cameraAnimator;
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
        playAnimation();
    }

    public void playAnimation() {
        mainCamera = Camera.main;
        cameraAnimator.SetTrigger("StartAnimation");
        Debug.Log("animation should play");
    }
}

