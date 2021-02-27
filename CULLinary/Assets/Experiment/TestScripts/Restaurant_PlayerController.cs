using UnityEngine;
using UnityEngine.AI;

public class Restaurant_PlayerController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public float wasdAvoidanceRadius = 0.0f;
    public bool clickedCooking = false;

    bool keyInput = false;
    float defaultRadius;

    void Start() {
        defaultRadius = agent.radius;
    }

    // Update is called once per frame
    void Update()
    {
        // Stop spinning at the destination
        if ((agent.transform.position - agent.destination).magnitude < 0.5f) {
            agent.isStopped = true;
        } else {
            agent.isStopped = false;
        }

        if (Input.GetMouseButton(0))
        {
            agent.radius = defaultRadius;
            keyInput = false;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //Move our agent
                agent.SetDestination(hit.point);

                if (hit.collider != null && hit.collider.gameObject.tag == "CookingStation") {
                    clickedCooking = true;
                } else {
                    clickedCooking = false;
                }
            }
        }

        Vector3 movementOffset = new Vector3(0.0f, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.W)) {
            keyInput = true;
            movementOffset = movementOffset + new Vector3(0.0f, 0.0f, -5.0f);
        }
        if (Input.GetKey(KeyCode.A)) {
            keyInput = true;
            movementOffset = movementOffset + new Vector3(5.0f, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.S)) {
            keyInput = true;
            movementOffset = movementOffset + new Vector3(0.0f, 0.0f, 5.0f);
        }
        if (Input.GetKey(KeyCode.D)) {
            keyInput = true;
            movementOffset = movementOffset + new Vector3(-5.0f, 0.0f, 0.0f);
        }
        if (keyInput) { // maybe should call an API here to "interact" with the object that was clicked instead (interact with pan / interact with table etc)
            agent.radius = wasdAvoidanceRadius;
            agent.SetDestination(agent.transform.position + movementOffset); // Might have to edit this because if it's an navmesh obstacle (ie. table/chair) then it will override the collider??
        }
    }
}
