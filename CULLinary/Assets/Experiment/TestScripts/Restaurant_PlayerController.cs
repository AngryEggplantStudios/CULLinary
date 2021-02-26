using UnityEngine;
using UnityEngine.AI;

public class Restaurant_PlayerController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public float wasdAvoidanceRadius = 0.0f;
    
    bool keyInput = false;
    float defaultRadius;

    void Start() {
        defaultRadius = agent.radius;
    }

    // Update is called once per frame
    void Update()
    {
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
                Debug.Log("Moving the agent!");
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
        if (keyInput) {
            agent.radius = wasdAvoidanceRadius;
            agent.SetDestination(agent.transform.position + movementOffset);
        }
    }
}
