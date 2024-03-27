using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeedMultiplier = 1.5f;
    public float slideSpeed = 10f;
    public float slideDuration = 1f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public LayerMask groundMask;

    private CharacterController characterController;
    private bool isGrounded;
    private float verticalVelocity;
    private bool isSliding;
    private float slideTimer;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);

        // Movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Sprinting input
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && verticalInput > 0;

        // Sliding input
        bool isCrouching = Input.GetKey(KeyCode.LeftControl);
        if (isCrouching && isGrounded && !isSliding)
        {
            StartSlide();
        }

        // Calculate movement speed based on sprinting and sliding
        float currentSpeed = moveSpeed;
        if (isSprinting && !isSliding)
        {
            currentSpeed *= sprintSpeedMultiplier;
        }
        else if (isSliding)
        {
            currentSpeed = slideSpeed;
        }

        // Apply movement
        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        characterController.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);

        // Jumping input
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = jumpForce;
        }

        // Apply gravity
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Apply vertical velocity
        Vector3 verticalMove = transform.up * verticalVelocity;
        characterController.Move(verticalMove * Time.deltaTime);

        // Update sliding state and timer
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f || !isGrounded)
            {
                EndSlide();
            }
        }
    }


    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        characterController.height /= 2f; // Halve the character's height while sliding
    }

    void EndSlide()
    {
        isSliding = false;
        characterController.height *= 2f; // Restore the character's original height
    }
}

