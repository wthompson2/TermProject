using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BossFightManager : MonoBehaviour
{
    int deathCount = 0;
    float completedTime = 0;
    bool playedSound = false;

    public AudioClip levelcomplete;
    private AudioSource actionSound;

    void Start()
    {
        actionSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (deathCount == 6)
        {
            if (!playedSound)
            {
                actionSound.PlayOneShot(levelcomplete, 1.0f);
                Cursor.lockState = CursorLockMode.Confined;
            }
            completedTimer();
            playedSound = true;
        }
    }

    public void enemiesKilled()
    {
        deathCount++;
    }

    public void completedTimer()
    {
        completedTime += Time.deltaTime;
        if (completedTime > 3)
        {
            SceneController.nextLevel();
        }
    }
}
