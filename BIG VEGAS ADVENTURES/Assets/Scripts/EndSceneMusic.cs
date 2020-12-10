using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("BossTheme").GetComponent<Music>().StopMusic();
        GameObject.FindGameObjectWithTag("EndTheme").GetComponent<Music>().PlayMusic();
    }
}
