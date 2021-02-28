using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Restaurant_CustomerController : MonoBehaviour
{
    public GameObject customer;
    private void Start()
    {
        // Play the sitting down animation  
        Animator animator = gameObject.GetComponent<Animator>();
        animator.SetBool("SitDown", true);
        Debug.Log("animator is: " + animator.ToString());
    }

}
