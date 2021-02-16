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
    [SerializeField] private float turnSmoothTime;


    //Cameras
    [SerializeField] private Transform cam; //Main camera
    [SerializeField] private GameObject tpCam; //Third Person Camera
    [SerializeField] private GameObject aimCam; //Aim Camera

    private float moveSpeed = 1.0f;
    private Vector3 direction;
    private Vector3 normalizedDirection;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private float turnSmoothVelocity;
    private CharacterController controller;
    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Aim();
        Punch();
        Move();
        
    }

    private void Punch()
    {
        if (Input.GetKey(KeyCode.F))
        {
            isAttacking = true;
            animator.SetBool("isPunch", true);
        } else
        {
            isAttacking = false;
            animator.SetBool("isPunch", false);
        }
    }

    private void Aim()
    {
        if (Input.GetMouseButton(1))
        {
            isAttacking = true;
            tpCam.SetActive(false);
            aimCam.SetActive(true);
            animator.SetBool("isAim", true);
            
        } else
        {
            isAttacking = false;
            tpCam.SetActive(true);
            aimCam.SetActive(false);
            animator.SetBool("isAim", false);
        }
    }

    private void Move()
    {
        //Get input
        float moveVertical = Input.GetAxisRaw("Vertical");
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
        normalizedDirection = direction.normalized;

        float targetAngle = Mathf.Atan2(normalizedDirection.x, normalizedDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        moveDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;

        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (isGrounded && !isAttacking)
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

        //Rotation
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

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
