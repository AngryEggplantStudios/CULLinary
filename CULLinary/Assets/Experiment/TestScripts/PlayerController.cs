using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Speed
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    //Cameras
    [SerializeField] private GameObject cam; //Main camera

    //Directions
    private Vector3 direction;
    private Vector3 normalizedDirection;
    private Vector3 moveDirection;

    //Defaults
    private float moveSpeed = 1.0f;
    private float turnSpeed = 10.0f;
    private Animator animator;
    private CharacterController controller;
    private PlayerAim playerAim;
    private PlayerRegularAttack playerRegularAttack;
    private GameObject playerBody;

    void Start()
    {
        playerAim = GetComponent<PlayerAim>();
        playerRegularAttack = GetComponent<PlayerRegularAttack>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        playerBody = GameObject.FindWithTag("PlayerBody");
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
        
        if (!playerAim.GetIsAiming() && !playerRegularAttack.GetIsRegularAttack())
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

            //Orientation
            if (direction != Vector3.zero)
            {
                playerBody.transform.rotation = Quaternion.Slerp(playerBody.transform.rotation, Quaternion.LookRotation(normalizedDirection), Time.deltaTime * turnSpeed);
            }
        }
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
}
