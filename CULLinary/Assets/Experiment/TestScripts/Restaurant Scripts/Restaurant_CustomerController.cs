using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Restaurant_CustomerController : MonoBehaviour
{
    public GameObject customer;
    public GameObject orderUI; // order customer places - rn is just text but can replace w image of the dish later(?)
    public GameObject serveFoodLocation;
    public Text foodText;

    [SerializeField]
    public string[] dishNames;

    private void Start()
    {
        // Play the sitting down animation  
        Animator animator = gameObject.GetComponent<Animator>();
        animator.SetBool("SitDown", true);
        Debug.Log("animator is: " + animator.ToString());

        int idx = Random.Range(0, dishNames.Length);
        foodText.text = dishNames[idx];
        orderUI.SetActive(true);
    }

    private void Update()
    {
        if (serveFoodLocation.transform.childCount != 0) // ie. player received the food
        {
            ReceiveFood();
        }
    }

    // Play the eating anim for a fixed duration
    public void ReceiveFood()
    {
        // Debug.Log("customer eating now");
        orderUI.SetActive(false);

        // play eating anim

        StartCoroutine(TimeToLeave());
    }

    IEnumerator TimeToLeave()
    {
        yield return new WaitForSeconds(2);

        Leave();
    }

    // Destroy the customer once they are done eating
    void Leave()
    {
        // Debug.Log("customer leaving now");
        Animator animator = gameObject.GetComponent<Animator>();
        animator.SetBool("SitDown", false); //wanted customer to play stand up anim b4 leaving but this one doesn't work :')

        StartCoroutine(DestroyCustomer());
    } 

    IEnumerator DestroyCustomer()
    {
        CustomerSpawner customerSpawner = GameObject.Find("Customer Spawner").GetComponent<CustomerSpawner>();
        
        yield return new WaitForSeconds(2);

        customerSpawner.currCustNum--; // -1 from total no. of curr cust because this customer left
        Destroy(customer); 
    }

}
