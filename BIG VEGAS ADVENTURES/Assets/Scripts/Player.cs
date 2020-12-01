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
    int currentHealth; 
    private CharacterController characterController;
    private Animator animator;
    private float gravity = -9.81f;
    public float jumpHeight = 4f;
    private PlayerMovementInfo playerMovement;
    private CollisionFlags lastMove;
    bool isGrounded;
    float velocityY;
    bool dead;
    float elastHit;
    bool beenHit;
    float lastHit;
    float respawnTime = 0; 
    Vector3 airMovement;
    Vector3 impact = Vector3.zero;

    // private static bool CameraDisabled = false;

    // public GameObject pauseMenu;

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
      
        currentHealth = totalHealth;
        lastHit = 0;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     Instantiate(pauseMenu);
        //     CameraDisabled = true;
        // }

        if(dead)
        {
            RespawnTimer(); 
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
            playerMovement.baseSpeed = 0;
            animator.SetBool("Dead", true);
            dead = true;
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            currentHealth--;
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
            animator.SetBool("isHitting", true);
            PlayerInventory.setHitting(true);
        }
        else
        {
            animator.SetBool("isHitting", false);
            PlayerInventory.setHitting(false);
        }

        airMovement.y += gravity * Time.deltaTime;
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Jumping", true);
            airMovement.y += Mathf.Sqrt(-2 * gravity * jumpHeight);
        }
        else
        {
            animator.SetBool("Jumping", false);
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

        if (running)
        {
            playerMovement.speed = playerMovement.baseSpeed * playerMovement.runningAmplifier;
        }
        else
        {
            playerMovement.speed = playerMovement.baseSpeed;

            playerMovement.forwardAndBackward = playerMovement.forwardAndBackward / 2.0f;
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
                currentHealth--;
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
            PlayerInventory.Add(other.gameObject);
        }
        else if(other.CompareTag("Bread"))
        {
            PlayerInventory.Add(other.gameObject);
        }
        else if(other.CompareTag("Peanut Butter"))
        {
            PlayerInventory.Add(other.gameObject);
        }
        else if(other.CompareTag("Hair"))
        {
            if (currentHealth != totalHealth)
            {
                currentHealth = totalHealth;
                other.gameObject.SetActive(false);
            }
        }
        else if (other.CompareTag("HairSpray"))
        {
            if (currentHealth != 3)
            {
                currentHealth += 1;
                other.gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.CompareTag("Enemy") && !beenHit)
        {
            currentHealth--;
            Vector3 knockBackDistance = other.gameObject.transform.position;
            knockBackDistance *= 3;
            addKnockback(knockBackDistance, 20);
            beenHit = true;
        }
    }

    public void RespawnTimer()
    {
        respawnTime += Time.deltaTime;
        if (respawnTime > 3)
        {
            SceneController.Restart();

        }
    }

    // public static void SetCameraDisabled(bool disabled)
    // {
    //     CameraDisabled = disabled;
    // }

}
