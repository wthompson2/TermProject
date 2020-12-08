using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningWalkingAudio : MonoBehaviour
{
    public AudioClip run;
    public AudioClip walk;
    
    private AudioSource actionSound;

    public void StartActionSound()
    {
        actionSound = GetComponent<AudioSource>();
    }

    public void PlayMovementAudio(int action, bool continuouslyRunning, bool continuouslyWalking, bool playedRunningSound, bool playedWalkingSound) // 0 is play walk, 1 is play run, -1 is stop audio
    {
        if (action == 1)
        {
            if (!actionSound.isPlaying && continuouslyRunning)
            {
                actionSound.PlayOneShot(run, 0.5f);
            }

            if (actionSound.isPlaying && !playedRunningSound)
            {
                actionSound.Stop();
            }

            if (!playedRunningSound)
            {
                actionSound.PlayOneShot(run, 0.5f);
            }
        }
        else if (action == 0)
        {
            if (!actionSound.isPlaying && continuouslyWalking)
            {
                actionSound.PlayOneShot(walk, 0.5f);
            }

            if (actionSound.isPlaying && !playedWalkingSound)
            {
                actionSound.Stop();
            }

            if (!playedWalkingSound)
            {
                actionSound.PlayOneShot(walk, 0.5f);
            }
        }
        else
        {
            actionSound.Stop();
        }
    }

    
}
