using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Restaurant_PlayerController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public Animator animator;
    
    public float wasdAvoidanceRadius = 0.86f;

    public ServingController controller;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float speed = 5.0f;

    bool keyInput = false;
    float defaultRadius;

    bool reachedDest = false;

    void Start() {
        defaultRadius = agent.radius;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("resetKeyInput");

        // Stop spinning at the destination
        if ((agent.transform.position - agent.destination).magnitude < 0.9f) {
            if (reachedDest == true)
            {
                // do nothing?
            } else // already triggered the reachedDest bool
            {
                reachedDest = true;
                
                agent.isStopped = true;
            }          
        } else {
            // do nothing?
        }

        if (Input.GetMouseButton(0))
        {
            agent.isStopped = false;
            reachedDest = false;  // reset values to allow movement

            agent.radius = 0.86f; // defaultRadius;
            keyInput = false;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                /*
                 *  CAN DELETE THIS COMMENTED PART LATER IF WE NOT GONNA USE MOUSE INPUT FOR MOVEMENT
                 *  
                // to check if path is possible 
                NavMeshPath path = new NavMeshPath();
                Vector3 targetPos = hit.point;
                agent.CalculatePath(targetPos, path);

                // Might have to edit this because if it's an navmesh obstacle (ie. table/chair) then it will override the collider??
                // Check if path is possible before moving the agent (player)
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    //Move our agent
                    if (controller.holdingItem == true)
                        animator.SetBool("walkingWithFood", true); // apply animation accordingly 
                    else
                        animator.SetBool("walking", true); 
                    agent.SetDestination(targetPos);
                }*/

                // Checking if cooking station is selected
                //if (hit.collider != null && hit.collider.gameObject.tag == "CookingStation") {
                //    hit.collider.gameObject.GetComponent<CookingStation>().Cook();
                //}
            }
        }
        
        Vector3 movementOffset = new Vector3(0.0f, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.W)) {
            keyInput = true;
            movementOffset = movementOffset + new Vector3(0.0f, 0.0f, -1.0f);
        }
        if (Input.GetKey(KeyCode.A)) {
            keyInput = true;
            movementOffset = movementOffset + new Vector3(1.0f, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.S)) {
            keyInput = true;
            movementOffset = movementOffset + new Vector3(0.0f, 0.0f, 1.0f);
        }
        if (Input.GetKey(KeyCode.D)) {
            keyInput = true;
            movementOffset = movementOffset + new Vector3(-1.0f, 0.0f, 0.0f);
        }
        if (keyInput) { // maybe should call an API here to "interact" with the object that was clicked instead (interact with pan / interact with table etc)
            agent.radius = wasdAvoidanceRadius;
            agent.isStopped = false;
            reachedDest = false; // reset values to allow movement

            // to check if path is possible 
            NavMeshPath path = new NavMeshPath();
            Vector3 targetPos = agent.transform.position + movementOffset;
            agent.CalculatePath(targetPos, path);

            // Might have to edit this because if it's an navmesh obstacle (ie. table/chair) then it will override the collider??
            // Check if path is possible before moving the agent (player)
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.GetComponent<NavMeshAgent>().speed = 20.0f; // Adjust the speed of navmesh agent (player)
                agent.SetDestination(targetPos);
            }
        }
        
        // Handle animations
        animator.SetBool("isWalking", movementOffset != Vector3.zero);
        animator.SetBool("hasFood", controller.holdingItem);
    }

    IEnumerator resetKeyInput()
    {
        yield return new WaitForSeconds(3);

        keyInput = false;
    }
}
