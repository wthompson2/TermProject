﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float baseSpeed;
    public float runningAmplifier;

    float currentSpeed;

    public bool lockCursor;

    int totalHealth = 3;
    int previousHealth;
    int currentHealth; 
    private CharacterController characterController;
    private Animator animator;
    private float gravity = -9.81f;
    public float jumpHeight = 4f;
    private PlayerMovementInfo playerMovement;
    bool isGrounded;
    bool jumping = false;
    bool dead;
    bool beenHit;
    float lastHit;
    float respawnTime = 0;
    float completedTime;
    bool completed;
    float lastAttack;
    Vector3 airMovement;
    Vector3 impact = Vector3.zero;
    bool playedDeathSound = false;
    bool playedWalkingSound = false;
    bool playedRunningSound = false;
    bool continuouslyWalking = false;
    bool continuouslyRunning = false;

    public GameObject pauseMenu;
    // public GameObject healthUI;
    public Health healthScript;
    public RunningWalkingAudio runningWalking;

    // public AudioClip run;
    public AudioClip oof;
    public AudioClip levelcomplete;
    public AudioClip killed;
    public AudioClip item;
    public AudioClip swing;
    public AudioClip jump;
    // public AudioClip walk;

    private AudioSource actionSound;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInventory.Clear();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        characterController.minMoveDistance = 0.0f;
        playerMovement = new PlayerMovementInfo();
        playerMovement.baseSpeed = baseSpeed;
        playerMovement.runningAmplifier = runningAmplifier;
        PlayerInventory.setHitting(false); 
        currentHealth = totalHealth;
        lastHit = 0;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        actionSound = GetComponent<AudioSource>();
        runningWalking.StartActionSound();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(pauseMenu);
            Cursor.lockState = CursorLockMode.Confined;
        }

        if(dead)
        {
            // actionSound.PlayOneShot(killed, 1.0f);
            PlayerInventory.Clear();
            RespawnTimer();
        }
        if(completed)
        {
            completedTimer();
        }
        isGrounded = characterController.isGrounded;

        if (isGrounded && airMovement.y < 0)
        {
            airMovement.y = 0f;
        }
        ProcessInput();
        PerformBlendTreeAnimation();

        CalculateDirectionAndDistance();


        PerformPhysicalMovement();
        if(currentHealth == 0)
        {
            if (!playedDeathSound)
            {
                playedDeathSound = true;
                actionSound.PlayOneShot(killed, 0.5f);
            }
            playerMovement.baseSpeed = 0;
            animator.SetBool("Dead", true);
            dead = true;
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            actionSound.PlayOneShot(oof, 0.5f);
            currentHealth--;
            healthScript.HandleHealthDepletion(currentHealth);
            Debug.Log(currentHealth);
        }

        RotatePlayerWithCamera();
 
        if (impact.magnitude > .2f)
        {
            
            characterController.Move(impact * Time.deltaTime);
        }
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
        if (beenHit)
        {
            lastHit += Time.deltaTime;
            if (lastHit > 2)
            {
                beenHit = false;
                lastHit = 0;
            }
        }
        if (isGrounded && Input.GetMouseButtonDown(0))
        {
            actionSound.PlayOneShot(swing, .5f);
            PlayerInventory.setHitting(true);
            animator.SetBool("isHitting", true);
            
        }
        else
        {
            animator.SetBool("isHitting", false);
            if (lastAttack > 1)
            {
                PlayerInventory.setHitting(false);
                lastAttack = 0;
            }
            if (PlayerInventory.getHitting())
            {
                lastAttack += Time.deltaTime;
                    }
           // Debug.Log(PlayerInventory.getHitting());
        }

        airMovement.y += gravity * Time.deltaTime;
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            actionSound.PlayOneShot(jump, 0.3f);
            animator.SetBool("Jumping", true);
            jumping = true;
            airMovement.y += Mathf.Sqrt(-2 * gravity * jumpHeight);
        }
        else
        {
            animator.SetBool("Jumping", false);
            jumping = false;
        }
        airMovement.y += gravity * Time.deltaTime;
        characterController.Move(airMovement * Time.deltaTime);
    }

    public void ProcessInput()
    {
        playerMovement.leftAndRight = Input.GetAxis("Horizontal"); // A and D
        playerMovement.forwardAndBackward = Input.GetAxis("Vertical"); // W and S

        playerMovement.movingForwards = playerMovement.forwardAndBackward > 0.0f;
        playerMovement.movingBackwards = playerMovement.forwardAndBackward < 0.0f;

        bool running = (playerMovement.movingForwards && Input.GetKey(KeyCode.LeftShift))
        || (!playerMovement.movingBackwards && (playerMovement.leftAndRight > 0.0f || playerMovement.leftAndRight < 0.0f));

        bool walking = (playerMovement.movingForwards && !Input.GetKey(KeyCode.LeftShift)) || playerMovement.movingBackwards;

        if (running)
        {
            if (!actionSound.isPlaying && isGrounded && continuouslyRunning)
            {
                runningWalking.PlayMovementAudio(1, continuouslyRunning, continuouslyWalking, playedRunningSound, playedWalkingSound); // actionSound.PlayOneShot(run, 0.5f);
            }

            if (actionSound.isPlaying && isGrounded && !playedRunningSound && !jumping)
            {
                runningWalking.PlayMovementAudio(-1, continuouslyRunning, continuouslyWalking, playedRunningSound, playedWalkingSound); // actionSound.Stop();
            }

            if (!playedRunningSound && isGrounded)
            {
                runningWalking.PlayMovementAudio(1, continuouslyRunning, continuouslyWalking, playedRunningSound, playedWalkingSound); // actionSound.PlayOneShot(run, 0.5f);
                playedRunningSound = true;
            }

            playerMovement.speed = playerMovement.baseSpeed * playerMovement.runningAmplifier;

            playedWalkingSound = false;
            continuouslyWalking = false;
            continuouslyRunning = true;
        }
        else if (walking)
        {
            if (!actionSound.isPlaying && isGrounded && continuouslyWalking)
            {
                runningWalking.PlayMovementAudio(0, continuouslyRunning, continuouslyWalking, playedRunningSound, playedWalkingSound); // actionSound.PlayOneShot(walk, 0.5f);
            }

            if (actionSound.isPlaying && isGrounded && !playedWalkingSound && !jumping)
            {
                runningWalking.PlayMovementAudio(-1, continuouslyRunning, continuouslyWalking, playedRunningSound, playedWalkingSound); // actionSound.Stop();
            }

            if (!playedWalkingSound && (playerMovement.movingForwards || playerMovement.movingBackwards) && isGrounded)
            {
                runningWalking.PlayMovementAudio(0, continuouslyRunning, continuouslyWalking, playedRunningSound, playedWalkingSound); // actionSound.PlayOneShot(walk, 0.5f);
                playedWalkingSound = true;
            }

            playerMovement.speed = playerMovement.baseSpeed;

            playerMovement.forwardAndBackward = playerMovement.forwardAndBackward / 2.0f;

            playedRunningSound = false;
            continuouslyRunning = false;
            continuouslyWalking = true;
        }
        else
        {
            runningWalking.PlayMovementAudio(-1, continuouslyRunning, continuouslyWalking, playedRunningSound, playedWalkingSound); // actionSound.Stop();
            continuouslyRunning = false;
            continuouslyWalking = false;
        }

        if (!running)
        {
            playedRunningSound = false;
        }
        if (!walking)
        {
            playedWalkingSound = false;
        }

    }

    public void PerformBlendTreeAnimation()
    {
        float leftAndRight = playerMovement.leftAndRight;

        if (playerMovement.movingBackwards)
        {
            leftAndRight = 0.0f;
        }

        animator.SetFloat("leftAndRight", leftAndRight);
        animator.SetFloat("forwardAndBackward", playerMovement.forwardAndBackward);
    }

    public void CalculateDirectionAndDistance()
    {
        Vector3 moveDirectionForward = transform.forward * playerMovement.forwardAndBackward;
        Vector3 moveDirectionSide = transform.right * playerMovement.leftAndRight;


        playerMovement.direction = moveDirectionForward + moveDirectionSide;
        playerMovement.normalizedDirection = playerMovement.direction.normalized;

        playerMovement.distance = (playerMovement.normalizedDirection) * playerMovement.speed * Time.deltaTime;

    }

    public void PerformPhysicalMovement()
    {
        characterController.Move(playerMovement.distance);
    }
    public void addKnockback(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0)
        {
            dir.y = -dir.y;
        }
        impact += dir.normalized * force;
        impact.y += 10;
    }


    public void RotatePlayerWithCamera()
    {
        Vector3 rotation;

        if (Input.GetKey(KeyCode.T))
        {
            return;
        }
        else
        {
            rotation = Camera.main.transform.eulerAngles;
            rotation.x = 0;
            rotation.z = 0;

            transform.eulerAngles = rotation;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        {
            if (hit.gameObject.CompareTag("Enemy") && !beenHit)
            {
                actionSound.PlayOneShot(oof, 0.5f);
                currentHealth--;
                healthScript.HandleHealthDepletion(currentHealth);
                Vector3 knockBackDistance = hit.transform.position;
                knockBackDistance *= 3; 
                addKnockback(knockBackDistance, 20);
                beenHit = true; 
            }
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("banana"))
        {
            actionSound.PlayOneShot(item, 0.5f);
            PlayerInventory.Add(other.gameObject);
            checkAllCollected();

        }
        else if(other.CompareTag("Bread"))
        {
            actionSound.PlayOneShot(item, 0.5f);
            PlayerInventory.Add(other.gameObject);
            checkAllCollected();
        }
        else if(other.CompareTag("Peanut Butter"))
        {
            actionSound.PlayOneShot(item, 0.5f);
            PlayerInventory.Add(other.gameObject);
            checkAllCollected();
        }
        else if(other.CompareTag("Hair"))
        {
            if (currentHealth != totalHealth)
            {
                actionSound.PlayOneShot(item, 0.5f);
                previousHealth = currentHealth;
                currentHealth = totalHealth;
                healthScript.HandleHealthGained(previousHealth, 1); // 0 is hairspray and 1 is hair
                other.gameObject.SetActive(false);
            }
        }
        else if (other.CompareTag("HairSpray"))
        {
            if (currentHealth != 3)
            {
                actionSound.PlayOneShot(item, 0.5f);
                previousHealth = currentHealth;
                currentHealth += 1;
                healthScript.HandleHealthGained(previousHealth, 0); // 0 is hairspray and 1 is hair
                other.gameObject.SetActive(false);
            }
        }
     
        else if(other.gameObject.CompareTag("DeadZone"))
        {
            dead = true; 
        }
    }

    public void RespawnTimer()
    {
        respawnTime += Time.deltaTime;
        if (respawnTime > 3)
        {
            SceneController.Restart();
            playedDeathSound = false;
            healthScript.HandleHealthRespawn(currentHealth);
        }
    }
    public void completedTimer()
    {
        completedTime += Time.deltaTime;
        if (completedTime > 3)
        {
            healthScript.HandleHealthRespawn(currentHealth);
            SceneController.nextLevel();
        }
    }

    public bool checkAllCollected()
    {
        GameObject[] inventory = PlayerInventory.getInventory().ToArray();
        bool hasBread = false;
        bool hasBanana = false;
        bool hasPB = false;
        Debug.Log(inventory.Length);
        if (inventory.Length == 3)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                GameObject temp =(GameObject) inventory.GetValue(i);
                Debug.Log(temp); 
                if(temp.CompareTag("banana"))
                {
                    hasBanana = true; 
                }
                else if(temp.CompareTag("Bread"))
                {
                    hasBread = true; 
                }
                else if (temp.CompareTag("Peanut Butter"))
                {
                    hasPB = true;
                }

            }
            
            if(hasBread && hasPB && hasBanana)
            {
                actionSound.PlayOneShot(levelcomplete, 1.0f);
                completed =  true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
        return completed; 
        
    }
}