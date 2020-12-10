using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCollisionBossFight : MonoBehaviour
{
    bool hitEnemy;
    float timePassed;

    public AudioClip hit;
    public AudioClip destroy;

    private AudioSource actionSound;

    // Start is called before the first frame update
    void Start()
    {
        hitEnemy = false;
        actionSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hitEnemy)
        {
            //actionSound.PlayOneShot(hit, 0.3f);

            if(timePassed > 1)
            {
                hitEnemy = false;
               
            }
            timePassed += Time.deltaTime;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hitEnemy && PlayerInventory.getHitting())
        {
            actionSound.PlayOneShot(hit, 0.3f);
            Debug.Log("hit enemy");
            hitEnemy = true;
            collision.gameObject.GetComponent<EnemyInfoBossFight>().decrementHealth();
            
        }
        if(collision.gameObject.CompareTag("Destructable") && PlayerInventory.getHitting())
        {
            actionSound.PlayOneShot(destroy, 0.3f);
            collision.gameObject.SetActive(false); 
        }
    }
}
