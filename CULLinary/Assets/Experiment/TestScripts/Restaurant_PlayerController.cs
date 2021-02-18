using UnityEngine;
using UnityEngine.AI;

public class Restaurant_PlayerController : MonoBehaviour
{
    public Camera cam;

    public NavMeshAgent agent;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //Move our agent
                agent.SetDestination(hit.point);
                Debug.Log("Moving the agent!");
            }
        }
    }
}
