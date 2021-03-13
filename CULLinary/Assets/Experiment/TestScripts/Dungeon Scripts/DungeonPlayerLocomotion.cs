using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerLocomotion : DungeonPlayerAction {

    private Animator animator;
    private DungeonPlayerController dungeonPlayerController;
    private CharacterController controller;

    [SerializeField] private GameObject playerBody;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip stepSound;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Awake()
    {
        dungeonPlayerController = GetComponent<DungeonPlayerController>();
        dungeonPlayerController.OnPlayerMove += Move;
        dungeonPlayerController.OnPlayerRotate += Rotate;
    }

    private void Move(Vector3 direction, float speed, float animValue, bool isMoving=true)
    {
        animator.SetFloat("Speed", animValue, 0.1f, Time.deltaTime);
        if (isMoving) {
            controller.Move(direction.normalized * speed * Time.deltaTime);
        }
    }

    private void Rotate(Vector3 direction, float speed)
    {
        playerBody.transform.rotation = Quaternion.Slerp(playerBody.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * speed);
    }

    private void OnDestroy()
    {
        dungeonPlayerController.OnPlayerMove -= Move;
        dungeonPlayerController.OnPlayerRotate -= Rotate;
    }

    public void StepSound()
    {
        audioSource.clip = stepSound;
        audioSource.Play();
    }
}
