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
        Debug.Log("Resume is clicked");
        Time.timeScale = 1;
        Destroy(gameObject);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void HandleRestartButtonOnClickEvent()
    {
        Debug.Log("Restart is clicked");
        Time.timeScale = 1;
        Destroy(gameObject);
        SceneController.Restart();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void HandleQuitButtonOnClickEvent()
    {
        Debug.Log("Quit is clicked");
        Time.timeScale = 1;
        Destroy(gameObject);
        SceneController.GoToMenu(MenuName.Title);
        GameObject.FindGameObjectWithTag("GameTheme").GetComponent<Music>().StopMusic();
    }
}
