using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0;
    }

    public void HandleResumeButtonOnClickEvent()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
        // Player.SetCameraDisabled(false);
    }

    public void HandleRestartButtonOnClickEvent()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
        SceneController.GoToMenu(MenuName.Play);
    }

    public void HandleQuitButtonOnClickEvent()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
        SceneController.GoToMenu(MenuName.Title);
    }
}
