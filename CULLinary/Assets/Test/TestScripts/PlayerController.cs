using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float rotSpeed;
    
    
    private Vector3 moveDirection;
    private Vector3 rotateDirection;
    private Vector3 velocity;
    private CharacterController controller;
    private Animator animator;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        moveDirection = new Vector3(0.0f, 0.0f, moveVertical);
        rotateDirection = new Vector3(moveHorizontal, 0.0f, 0.0f);
        moveDirection = transform.TransformDirection(moveDirection);

        if (isGrounded)
        {
            if (moveDirection == Vector3.zero)
            {
                Idle();
            }
            else
            {
                Walk();
            }

            moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        controller.Move(moveDirection * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Quaternion toRotation = Quaternion.LookRotation(rotateDirection, Vector3.up);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotSpeed * Time.deltaTime);
        transform.Rotate(0, moveHorizontal * rotSpeed * Time.deltaTime, 0);
    }

    private void Idle()
    {
        animator.SetFloat("Speed", 0.0f, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        animator.SetFloat("Speed", 1.0f, 0.1f, Time.deltaTime);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }
}
