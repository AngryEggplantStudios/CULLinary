using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerLocomotion : DungeonPlayerAction {

    private Animator animator;
    private DungeonPlayerController dungeonPlayerController;
    private CharacterController controller;
    private GameObject player;

    [SerializeField] private GameObject playerBody;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepSounds;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Awake()
    {
        dungeonPlayerController = GetComponent<DungeonPlayerController>();
        dungeonPlayerController.OnPlayerMove += Move;
        dungeonPlayerController.OnPlayerRotate += Rotate;
        dungeonPlayerController.OnPlayerInteract += FaceWorldPosition;
    }

    private void Move(Vector3 direction, float speed, float animValue, bool isMoving=true)
    {
        if (!dungeonPlayerController.gameOverBool)
        {
            animator.SetFloat("Speed", animValue, 0.1f, Time.deltaTime);
            if (isMoving)
            {
                if (controller.collisionFlags != CollisionFlags.Below && controller.collisionFlags != CollisionFlags.None)
                {
                    if (player.transform.position.y > 0.01)
                    {
                        direction = new Vector3(direction.x, -0.30f, direction.z);
                    }
                }
                controller.Move(direction.normalized * speed * Time.deltaTime);
            }
        }

    }
    public void KnockBack(Vector3 direction, float speed, float animValue, bool isMoving = true)
    {
//        animator.SetFloat("Speed", animValue, 0.1f, Time.deltaTime);
        if (isMoving)
        {
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

    public void StepSound(AnimationEvent evt)
    {
        if (!dungeonPlayerController.gameOverBool)
        {
            if (evt.animatorClipInfo.weight > 0.5)
            {
                audioSource.clip = stepSounds[Random.Range(0, stepSounds.Length)];
                audioSource.Play();
            }
        }
    }

    public void FaceWorldPosition(Vector3 worldPosition, float speed)
    {
        Vector3 playerPosition = playerBody.transform.position;
        Vector3 lookAtVector = worldPosition - playerPosition;
        this.Rotate(new Vector3(lookAtVector.x, 0, lookAtVector.z), speed);
    }
}
