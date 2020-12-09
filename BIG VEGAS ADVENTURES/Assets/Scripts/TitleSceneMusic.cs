using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("TitleTheme").GetComponent<Music>().PlayMusic();
    }
}
