using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Restaurant_CustomerController : MonoBehaviour
{
    public GameObject customer;
    public GameObject orderUI; // order customer places - rn is just text but can replace w image of the dish later(?)
    public GameObject moneyText;
    public GameObject serveFoodLocation;
    public Text foodText;
    public Image dishImg;

    [SerializeField]
    public string[] dishNames;

    [SerializeField]
    public Sprite[] dishImages;

    private int idx = 0;
    private bool alrReceivedFood = false;
    public bool canBeSpokenTo = true;
    
    public AudioSource kachingSound;

    private void Start()
    {
        // Play the sitting down animation  
        Animator animator = gameObject.GetComponent<Animator>();
        animator.SetBool("SitDown", true);

        Scene currScene = SceneManager.GetActiveScene();
        if (currScene.buildIndex == (int)SceneIndexes.TUT_RETURN)
        {
            idx = 0; // always order eggplant in return_tut_restaurant scene
        } else
        {
            idx = Random.Range(0, dishNames.Length); // set the idx here, it was 0 by default
        }
        
        foodText.text = dishNames[idx];
        dishImg.sprite = dishImages[idx];
        orderUI.SetActive(true);
    }

    private void Update()
    {
        if (serveFoodLocation.transform.childCount != 0 && !alrReceivedFood) // ie. player received the food
        {
            ReceiveFood();
        }
    }

    // Check to see if already received the food
    public bool HasReceivedFood() {
        return alrReceivedFood;
    }

    public bool HasDialogue() {
        return canBeSpokenTo;
    }

    public void SetToNoDialogue() {
        canBeSpokenTo = false;
    }

    // Play the eating anim for a fixed duration
    public void ReceiveFood()
    {
        // Debug.Log("customer eating now");
        orderUI.SetActive(false);
        alrReceivedFood = true;

        UIController uiController = GameObject.Find("UI Controller").GetComponent<UIController>();

        // Play eating anim

        // Determine amount to pay and show UI of the score (based on whether they get the correct food or not)
        string dishReceived = serveFoodLocation.transform.GetChild(0).name;
        string correctDishName = dishNames[idx] + "(Clone)";
        if (dishReceived == correctDishName) // Player served the correct dish
        {
            // Debug.Log("You served the correct dish!");
            moneyText.GetComponent<Text>().text = "+$100";
            uiController.AddCorrectDishEarnings();
        }
        else // Player served the wrong dish
        {
            // Debug.Log("Customer wants: "+ correctDishName + " but received: " + dishReceived + " >:(");
            moneyText.GetComponent<Text>().text = "+$50";
            uiController.AddWrongDishEarnings(); 
        }

        moneyText.SetActive(true); // add anims to money text?
        kachingSound.Play();

        StartCoroutine(FadeMoneyText());
        StartCoroutine(TimeToLeave());
    }

    public IEnumerator FadeMoneyText()
    {
        yield return new WaitForSeconds(2);
        moneyText.SetActive(false);
    }

    // This can be interrupted if Customer is spoken to
    public IEnumerator TimeToLeave()
    {
        if (canBeSpokenTo) {
            for (float time = 0.0f; time < 4.0f; time = time + Time.deltaTime) {
                if (!canBeSpokenTo) {
                    // Customer is being spoken to, don't destroy yet
                    yield break;
                }
                yield return null;
            }
        } else {
            yield return new WaitForSeconds(2);
        }
        Leave();
    }

    // Destroy the customer once they are done eating
    void Leave()
    {
        // // Debug.Log("customer leaving now");
        // Animator animator = gameObject.GetComponent<Animator>();
        // animator.SetBool("SitDown", false); //wanted customer to play stand up anim b4 leaving but this one doesn't work :')
        // StartCoroutine(DestroyCustomer());
        Scene currScene = SceneManager.GetActiveScene();
        if (currScene.buildIndex != (int)SceneIndexes.FINALE)
            DestroyCustomer();
    } 

    void DestroyCustomer()
    {
        CustomerSpawner customerSpawner = GameObject.Find("Customer Spawner").GetComponent<CustomerSpawner>();
        
        // yield return new WaitForSeconds(2);

        customerSpawner.currCustNum--; // -1 from total no. of curr cust because this customer left
        Destroy(customer); 
    }

}
