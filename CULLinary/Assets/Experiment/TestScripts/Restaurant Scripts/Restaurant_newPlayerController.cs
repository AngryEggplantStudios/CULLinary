using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Restaurant_newPlayerController : MonoBehaviour
{
    public Camera cam;   
    public Animator animator;
        
    public ServingController servingController;

    // For Player Movement
    public CharacterController controller;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float speed = 10.0f;
    private Vector3 direction;
    private Vector3 moveDirection;


    private void Update()
    {
        // Get movement input
        float moveVertical = Input.GetAxisRaw("Vertical");
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        // Necessary calculations for movement 
        direction = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        // Handle animations
        animator.SetBool("isWalking", (moveVertical != 0.0f || moveHorizontal != 0.0f) );
        animator.SetBool("hasFood", servingController.holdingItem);

        // For cooking animation
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Checking if cooking station is selected
                if (hit.collider != null && hit.collider.gameObject.tag == "CookingStation")
                {
                    // Debug.Log("Clicked cooking station!");
                    hit.collider.gameObject.GetComponent<CookingStation>().Cook();
                }
            }
        }
    }
}
