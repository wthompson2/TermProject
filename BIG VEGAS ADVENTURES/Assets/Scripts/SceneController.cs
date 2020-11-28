using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class SceneController 
{
    public static void SceneSwitch()
    {
        SceneManager.LoadScene(0);
    }

    public static void nextLevel()
    {
       
        SceneManager.LoadScene(getCurrentSceneIndex() + 1);
    }

    public static void Restart()
    {

        SceneManager.LoadScene(getCurrentSceneIndex());
    }



    public static int getCurrentSceneIndex()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        return currentSceneIndex;
    }
}
