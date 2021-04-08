using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutFindPlayer : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent customer;
    public Animator customerAnimator;
    public Transform playerLocation;
    public float talkingRadius = 15.0f;
    // To rotate the player
    public DungeonPlayerLocomotion playerLocomotion;
    public float playerRotateSpeed = 5.0f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepSounds;

    public delegate void ReachedPlayerDelegate();

    private bool isPathfinding = true;
    private bool isTurning = false;
    private bool triggeredCallback = false;
    // for animation
    private Vector3 previousPosition;

    private event ReachedPlayerDelegate ReachedPlayerCallback;

    private void Start()
    {
        previousPosition = customer.transform.position;
    }

    private void Update()
    {
        if (isPathfinding) {
            if ((customer.transform.position - playerLocation.position).magnitude < talkingRadius) {
                isPathfinding = false;
                customer.SetDestination(playerLocation.position + talkingRadius *
                                        Vector3.Normalize(
                                            customer.transform.position - playerLocation.position));
                isTurning = true;
            } else if (Random.Range(0, 5) == 2) {
                // Called once every 5 frames at random
                customer.SetDestination(playerLocation.position);
            }
        }

        if (isTurning) {
            customer.transform.rotation =
                Quaternion.Slerp(customer.transform.rotation,
                                 Quaternion.LookRotation(playerLocation.position - customer.transform.position),
                                 Time.deltaTime * customer.angularSpeed * 0.025f);
            if (!triggeredCallback) {
                triggeredCallback = true;
                ReachedPlayerCallback.Invoke();
            } else {
                playerLocomotion.FaceWorldPosition(customer.transform.position, playerRotateSpeed);
            }
        }

        Vector3 currentMotion = transform.position - previousPosition;
        previousPosition = transform.position;
        customerAnimator.SetFloat("speed", currentMotion.magnitude / Time.deltaTime);
    }

    public void SetReachedPlayerCallback(ReachedPlayerDelegate del)
    {
        ReachedPlayerCallback = del;
    }

    public void StepSound(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5)
        {
            audioSource.clip = stepSounds[Random.Range(0, stepSounds.Length)];
            audioSource.Play();
        }
    }
}
