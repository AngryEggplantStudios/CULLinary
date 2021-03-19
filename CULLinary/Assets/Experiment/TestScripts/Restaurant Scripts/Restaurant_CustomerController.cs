using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Restaurant_CustomerController : MonoBehaviour
{
    public GameObject customer;
    public GameObject orderUI; // order customer places - rn is just text but can replace w image of the dish later(?)
    public GameObject moneyText;
    public GameObject serveFoodLocation;
    public Text foodText;

    [SerializeField]
    public string[] dishNames;

    private int amountToPay = 0;
    private int idx = 0;
    private bool alrReceivedFood = false;

    private void Start()
    {
        // Play the sitting down animation  
        Animator animator = gameObject.GetComponent<Animator>();
        animator.SetBool("SitDown", true);
        Debug.Log("animator is: " + animator.ToString());

        idx = Random.Range(0, dishNames.Length); // set the idx here, it was 0 by default
        foodText.text = dishNames[idx];
        orderUI.SetActive(true);
    }

    private void Update()
    {
        if (serveFoodLocation.transform.childCount != 0 && !alrReceivedFood) // ie. player received the food
        {
            ReceiveFood();
        }
    }

    // Play the eating anim for a fixed duration
    public void ReceiveFood()
    {
        // Debug.Log("customer eating now");
        orderUI.SetActive(false);
        alrReceivedFood = true;

        // Play eating anim

        // Determine amount to pay and show UI of the score (based on whether they get the correct food or not)
        string dishReceived = serveFoodLocation.transform.GetChild(0).name;
        string correctDishName = dishNames[idx] + "(Clone)";
        if (dishReceived == correctDishName) // Player served the correct dish
        {
            Debug.Log("You served the correct dish!");
            amountToPay = 100;
            moneyText.GetComponent<Text>().text = "+100";
        }
        else // Player served the wrong dish
        {
            Debug.Log("Customer wants: "+ correctDishName + " but received: " + dishReceived + " >:(");
            amountToPay = 50;
            moneyText.GetComponent<Text>().text = "+50";
        }

        moneyText.SetActive(true); // add anims to text?

        StartCoroutine(TimeToLeave());
    }

    IEnumerator TimeToLeave()
    {
        yield return new WaitForSeconds(2);

        moneyText.SetActive(false);

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
