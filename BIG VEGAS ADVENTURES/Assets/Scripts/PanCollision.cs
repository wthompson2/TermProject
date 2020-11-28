using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCollision : MonoBehaviour
{
    bool hitEnemy;
    float timePassed;
    // Start is called before the first frame update
    void Start()
    {
        hitEnemy = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(hitEnemy)
        {
            if(timePassed > 1)
            {
                hitEnemy = false;
               
            }
            timePassed += Time.deltaTime;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hitEnemy)
        {
            Debug.Log("hit enemy");
            hitEnemy = true;
            collision.transform.GetComponent<EnemyInfo>().decrementHealth();
            
        }
    }
}
