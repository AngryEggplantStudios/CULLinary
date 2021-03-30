
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In charge of transforming dish's location from counter to player, and then from player to customer
public class ServingController : MonoBehaviour
{
    public GameObject player;
    public Camera cam;

    public GameObject[] foodPrefabs;
    public Transform foodLocation;

    // For triggering dialogue after Customer is served
    public DialogueLoader dialogueLoader;
    // CookingStation to disable movement while talking
    // Will be renabled once dialogue is closed, by DialogueLoader
    public CookingStation movementController;

    public bool holdingItem = false;
    public float interactRange = 7.0f;

    private int currentHeldFood = -1;

    // Helper function to find the closest game object with a given tag to the player.
    // Only items within interactRange will be searched.
    // 
    // Adapted from https://docs.unity3d.com/ScriptReference/GameObject.FindGameObjectsWithTag.html
    private GameObject FindClosestWithTag(string tag)
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag(tag);
        GameObject closestItem = null;
        float currentSmallestSquaredDistance = interactRange * interactRange;
        Vector3 playerPosition = player.transform.position;
        foreach (GameObject i in items)
        {
            float squaredDistance =
                (i.transform.position - playerPosition).sqrMagnitude;
            if (squaredDistance < currentSmallestSquaredDistance)
            {
                closestItem = i;
                currentSmallestSquaredDistance = squaredDistance;
            }
        }
        return closestItem;
    }

    // Destroy food the player was carrying
    void DestroyHeldFood() {
        if (holdingItem) {
            GameObject holdingPos = foodLocation.gameObject;
            foreach (Transform child in holdingPos.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            holdingItem = false; // reset bool value since not carrying anything anymore 
        }
    }

    // Update is called once per frame
    void Update()
    {        
        if (!holdingItem && Keybinds.WasTriggered(Keybind.Interact))
        {
            GameObject closestFoodItem = FindClosestWithTag("FoodToServe");
            if (closestFoodItem)
            {
                // Pick up food from counter
                string selectedDish = closestFoodItem.name;
                selectedDish = selectedDish.Remove(selectedDish.Length - 7); // Remove the "(Clone)"


                int foodId = -2;
                /*
                if (selectedDish == ("plateDinner(Clone)") ) 
                    foodId = 0;
                else if (selectedDish == ("bowlBroth(Clone)") ) 
                    foodId = 1;
                else if (selectedDish == ("pizza(Clone)") ) 
                    foodId = 2;
                else if (selectedDish == ("burrito(Clone)") ) 
                    foodId = 3;
                */
                for (int i = 0; i < foodPrefabs.Length; i++)
                {
                    if (foodPrefabs[i].name == selectedDish)
                    {
                        foodId = i;
                        break;
                    }
                }
                CollectFood(foodId, foodLocation.position);
                Destroy(closestFoodItem); // comment this out if want unlimited servings of the dish after cooking
            } else {
                // Try interacting with closest Customer
                GameObject closestCustomer = FindClosestWithTag("Customer");
                // Serve the customer if carrying food
                if (closestCustomer)
                {
                    Restaurant_CustomerController customerController =
                        closestCustomer.GetComponent<Restaurant_CustomerController>();
                    // Only serve food to Customers not already eating
                    if (customerController.HasReceivedFood() && customerController.HasDialogue()) {
                        movementController.DisableMovementOfPlayer();
                        dialogueLoader.LoadAndRun(DialogueDatabase.GetRandomDialogue(), customerController);
                        customerController.SetToNoDialogue();
                    }
                }
            }
        }
        else if (holdingItem && Keybinds.WasTriggered(Keybind.Trash))
        {
            DestroyHeldFood();
        }
        else if (holdingItem && Keybinds.WasTriggered(Keybind.Interact))
        {
            GameObject closestCustomer = FindClosestWithTag("Customer");
            // Serve the customer if carrying food
            if (closestCustomer)
            {
                Restaurant_CustomerController customerController =
                    closestCustomer.GetComponent<Restaurant_CustomerController>();
                // Only serve food to Customers not already eating
                if (!customerController.HasReceivedFood()) {
                    ServeFood(closestCustomer, currentHeldFood);
                }
            }
        }
    }

    // Shift food so player looks like carrying the food
    // Remove food from counter accordingly
    void CollectFood(int id, Vector3 position)
    {
        holdingItem = true;
        currentHeldFood = id;
        GameObject plateDish = Instantiate(foodPrefabs[id], position, Quaternion.identity);
        plateDish.transform.SetParent(foodLocation);
    }

    // Shift food so player looks has served food to customer
    // Remove food from player's hands accordingly
    void ServeFood(GameObject customer, int foodId)
    {
        Transform serveLocation = customer.transform.Find("ServeFoodLocation");
        if (!serveLocation) {
            throw new InvalidOperationException("Error! Some item with \"Customer\" "
            + "tag found but not the actual prefab Customer");
        }
        GameObject plateDish = Instantiate(foodPrefabs[foodId], serveLocation.position, Quaternion.identity);
        plateDish.tag = "Untagged"; // Without this, player can pick up the food
                                    // while the customer is eating!
        plateDish.transform.SetParent(serveLocation);    
        DestroyHeldFood();
    }
}
