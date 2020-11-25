using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Script is working"); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hit enemy"); 
        }
    }
}
