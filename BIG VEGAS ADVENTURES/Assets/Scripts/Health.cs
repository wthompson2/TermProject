using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject Health1;
    public GameObject Health2;
    public GameObject Health3;

    // private GameObject parentObject;

    void Start()
    {
        // parentObject = this.gameObject;
        this.transform.SetParent(this.transform);
    }

    public void HandleHealthDepletion(int health)
    {
        if (health == 2)
        {
            Destroy(this.transform.GetChild(2).gameObject);
        }

        if (health == 1)
        {
            Destroy(this.transform.GetChild(1).gameObject);
        }

        if (health == 0)
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }
    }

    public void HandleHealthGained(int current, int item) // 0 is hairspray and 1 is hair
    {
        for (int i = 1; i <= 2; i++)
        {
            if (i == 1)
            {
                Instantiate(Health2);
                Debug.Log("Health Gained");
                if (item == 0)
                {
                    break;
                }
            }
            if (i == 2)
            {
                Instantiate(Health3);
                Debug.Log("Health Gained");
                if (item == 0)
                {
                    break;
                }
            }
        }
    }

    public void HandleHealthRespawn()
    {
        for (int i = 0; i <= 2; i++)
        {
            if (this.transform.GetChild(i) == null)
            {
                break;
            }
            else
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
        }
    }
}
