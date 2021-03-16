using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/**
 * Minimal version of Restaurant_newPlayerController.cs
 * without any movement functionality
 */
public class Restaurant_MinimalPlayerController : MonoBehaviour
{
    public Animator animator;       
    public ServingController servingController;
    
    private void Update()
    {
        // Get movement input
        float moveVertical = Input.GetAxisRaw("Vertical");
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        // Handle animations
        animator.SetBool("isWalking", (moveVertical != 0.0f || moveHorizontal != 0.0f) );
        animator.SetBool("hasFood", servingController.holdingItem);
    }
}
