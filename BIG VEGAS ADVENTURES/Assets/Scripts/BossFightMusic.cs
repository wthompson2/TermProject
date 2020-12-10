using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightMusic : MonoBehaviour
{
    void Start()
    {
        GameObject.FindGameObjectWithTag("GameTheme").GetComponent<Music>().StopMusic();
        GameObject.FindGameObjectWithTag("BossTheme").GetComponent<Music>().PlayMusic();
    }
}
