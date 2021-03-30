using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2;
    [SerializeField] float turningSpeed = 100; // deg/s
    [SerializeField] float stoppingDistance = 1;
    [Tooltip("Distance to stop rotating")]
    [SerializeField] float lookingDistance = 0.3f;
    [SerializeField] float meleeRange = 1;

    [SerializeField] Transform lowerJaw;
    [SerializeField] IKFootSolver leftFoot;
    [SerializeField] IKFootSolver rightFoot;

    Transform player;
    float originalY;
    float jawOriginalY;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalY = transform.position.y;
        jawOriginalY = lowerJaw.localPosition.y;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer > lookingDistance){
            slowlyLookAt(player);
        }

        if (distanceToPlayer > stoppingDistance){
            moveForward();
        }

        if (distanceToPlayer < meleeRange){
            stepOn(player);
        }

        // Bob head and jaw for demostration
        transform.position = new Vector3(
                transform.position.x,
                originalY + Mathf.Sin(Time.fixedTime * Mathf.PI * 1) * 0.2f,
                transform.position.z);
        lowerJaw.localPosition = new Vector3(
                lowerJaw.localPosition.x,
                jawOriginalY - Mathf.Abs(Mathf.Sin(Time.fixedTime * Mathf.PI * 2) * 0.01f),
                lowerJaw.localPosition.z);
    }

    void slowlyLookAt(Transform target)
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(Quaternion.LookRotation(target.position - transform.position).eulerAngles),
            Time.deltaTime * turningSpeed);
    }

    void moveForward()
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    // both feet must be on the ground
    // target must be above ground and within range
    void stepOn(Transform target)
    {
        // Check if both feet are on the ground
        if (leftFoot.IsMoving() || rightFoot.IsMoving())
        {
            return;
        }

        // Find target on ground
        Ray ray = new Ray(target.position, Vector3.down);
        if (!Physics.Raycast(ray, out RaycastHit info, 100, 1 << LayerMask.NameToLayer("Ground")))
        {
            Debug.Log("stepOn() target " + target.position + " is not above the ground");
            return;
        }
        
        // Find closer foot and step
        if (Vector3.Distance(leftFoot.currentPosition, info.point) < Vector3.Distance(rightFoot.currentPosition, info.point))
        {
            leftFoot.SetTarget(info.point, info.normal);
        }
        else
        {
            rightFoot.SetTarget(info.point, info.normal);
        }
    }
}
