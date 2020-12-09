using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject Health1;
    public GameObject Health2;
    public GameObject HealthFull;

    private GameObject clone1;
    private GameObject clone2;
    private GameObject cloneFull;

    void Start()
    {
        cloneFull = (GameObject)Instantiate (HealthFull, transform.position, Quaternion.identity);
    }

    public void HandleHealthDepletion(int health)
    {
        if (health == 2)
        {
            Destroy(cloneFull);
            clone2 = (GameObject)Instantiate (Health2, transform.position, Quaternion.identity);
        }

        if (health == 1)
        {
            Destroy(clone2);
            clone1 = (GameObject)Instantiate (Health1, transform.position, Quaternion.identity);
        }

        if (health == 0)
        {
            Destroy(clone1);
        }
    }

    public void HandleHealthGained(int current, int item) // 0 is hairspray and 1 is hair
    {
        if (current == 1)
        {
            Destroy(clone1);

            if (item == 0)
            {
                clone2 = (GameObject)Instantiate (Health2, transform.position, Quaternion.identity);
            }
            else
            {
                cloneFull = (GameObject)Instantiate (HealthFull, transform.position, Quaternion.identity);
            }
        }
        if (current == 2)
        {
            Destroy(clone2);
            cloneFull = (GameObject)Instantiate (HealthFull, transform.position, Quaternion.identity);
        }
    }

    public void HandleHealthRespawn(int currentHealth)
    {
        if (currentHealth == 3)
        {
            Destroy(cloneFull);
        }
        else if (currentHealth == 2)
        {
            Destroy(clone2);
        }
        else if (currentHealth == 1)
        {
            Destroy(clone1);
        }
    }
}
