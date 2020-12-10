using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class EnemyInfoBossFight : MonoBehaviour
{
    public int health = 2;
    public bool dead;

    public BossFightManager bossFight;

    private void Start()
    {
        dead = false;
    }
    private void Update()
    {
        dead = isDead();
        if(dead)
        {
            deSpawn();
            bossFight.enemiesKilled();
        }
    }
    public  void decrementHealth()
    {
        health--; 
        
    }

    public bool isDead()
    {
        bool dead = false;
        if (health  <= 0)
        {
            dead = true;
        }
        return dead; 
    }

    public void deSpawn()
    {
        this.gameObject.SetActive(false); 
    }


}

