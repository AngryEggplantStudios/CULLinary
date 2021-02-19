using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Speed
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    //Check ground
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    //height
    [SerializeField] private float jumpHeight;

    //Cameras
    [SerializeField] private GameObject cam; //Main camera

    //Directions
    private Vector3 direction;
    private Vector3 normalizedDirection;
    private Vector3 moveDirection;
    private Vector3 velocity;

    //Defaults
    private float moveSpeed = 1.0f;
    private Animator animator;
    private CharacterController controller;
    private PlayerAim playerAim;
    private PlayerRegularAttack playerRegularAttack;

    void Start()
    {
        playerAim = GetComponent<PlayerAim>();
        playerRegularAttack = GetComponent<PlayerRegularAttack>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float moveVertical = Input.GetAxisRaw("Vertical");
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
        normalizedDirection = direction.normalized;

        float targetAngle = Mathf.Atan2(normalizedDirection.x, normalizedDirection.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
        moveDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;

        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        if (isGrounded && !playerAim.GetIsAiming() && !playerRegularAttack.GetIsRegularAttack())
        {
            if (direction == Vector3.zero)
            {
                Idle();
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Run();
                }
                else
                {
                    Walk();
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    
    private void Idle()
    {
        animator.SetFloat("Speed", 0.0f, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        animator.SetFloat("Speed", 1.0f, 0.1f, Time.deltaTime);
        controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }
}
