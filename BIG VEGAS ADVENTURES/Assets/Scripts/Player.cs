using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rg;
    public float baseSpeed;
    public float runningAmplifier;

    public bool lockCursor;

    private CharacterController characterController;
    private Animator animator;

    private float airTime;
    private float gravity = -9.81f;
    private float jumpHeight = 1.0f; 
    private PlayerMovementInfo playerMovement;
    bool jumping;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        playerMovement = new PlayerMovementInfo();
        playerMovement.baseSpeed = baseSpeed;
        playerMovement.runningAmplifier = runningAmplifier;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        airTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.isGrounded && playerMovement.direction.y < 0)
        {
           // playerMovement.direction.y = 0f;
        }
        ProcessInput();
        PerformBlendTreeAnimation();

        CalculateDirectionAndDistance();
        if (!jumping)
        {
            PerformPhysicalMovement();
        }
        else
        {
            PerformJumpingMovement();
        }
        RotatePlayerWithCamera();
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

        playerMovement.distance = playerMovement.normalizedDirection * playerMovement.speed * Time.deltaTime;

        GroundPlayer();
    }

    public void PerformPhysicalMovement()
    {
        characterController.Move(playerMovement.distance);
    }
    public void PerformJumpingMovement()
    {
        characterController.Move(playerMovement.distance);
        Debug.Log("Performed Jump");
    }

    public void GroundPlayer()
    {
        Vector3 direction = playerMovement.normalizedDirection;
        if (characterController.isGrounded)
        {
            airTime = 0;
            direction.y = 0; 
            if(Input.GetKey(KeyCode.Space))
            {
                jumping = true;
                Debug.Log(jumping);
                direction.y += Mathf.Sqrt(jumpHeight * -1000000.0f * gravity);
                playerMovement.normalizedDirection = direction;
                playerMovement.distance = playerMovement.normalizedDirection * Time.deltaTime;
            }
        }
        else
        {
            jumping = false;
            Debug.Log(jumping);
            airTime += Time.deltaTime;
            direction.y += gravity * airTime;
            playerMovement.normalizedDirection = direction;
            playerMovement.distance = playerMovement.normalizedDirection * airTime;

        }
        Debug.Log("is grounded" + characterController.isGrounded);
        
      
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

    public void Jump()
    {
        /*  Debug.Log("Jumping");
          Vector3 direction = playerMovement.normalizedDirection;
          direction.y -= gravity * Time.deltaTime * 100;
          characterController.Move(direction * Time.deltaTime);
        */

        
    }

}
