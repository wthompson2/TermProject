﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("GameTheme").GetComponent<Music>().PlayMusic();
    }
}
