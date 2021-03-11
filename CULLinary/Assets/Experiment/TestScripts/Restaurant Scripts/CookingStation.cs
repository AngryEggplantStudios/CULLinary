using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CookingStation : MonoBehaviour
{
    [Header("For Cooking")] // For cooking anim
    public Restaurant_newPlayerController playerController;
    
    public Transform stationLocation;
    public float minimumDistance = 7.0f;
    public Animator animator;

    public bool isCooking = false;
    string dishToCook = "";

    [Header("Cooking Counter")] // For spawning food on the counter
    public GameObject[] spawnFoodAreas;
    int availableIdx = 0;

    public GameObject[] foodPrefabs;

    [Header("Cooking Progress Bar")] // Progress Bar variables
    public CookingProgressbar cookingProgressBar;
    public GameObject progressIcon;
    public Transform progressBarTransform;

    public Collider recipeActivator;

    [SerializeField] private float currentAmount;
    [SerializeField] private float speed;

    void Update()
    {
        // Stop cooking anim if player walks away halfway
        // Probably can remove this once we let player stop moving when cooking
        if (isCooking && !PlayerWithinRange()) 
        {
            isCooking = false;
        }
        animator.SetBool("isCooking", isCooking);

        // To update the fill of progress icon when player is cooking 
        if (isCooking)
        {
            if (progressIcon.activeSelf == false)
            {
                progressIcon.SetActive(true); // Show the icon only if cooking
            }
            else
            {
                FillUpBar(); // Start filling up the bar once it is active
            }
        }
    }

    // Filling up the bar (also kind of represents the whole cooking process player goes through to produce a dish)
    void FillUpBar()
    {
        if (currentAmount < 100)
        {
            currentAmount += speed * Time.deltaTime;
            recipeActivator.enabled = false; // Ensure player doesn't open another recipe menu instance when something is currently cooking
        }
        else
        {
            
            //cookingNow = false;
            progressIcon.SetActive(false);      // Hide timer once progress = 100%
            currentAmount = 0;                  // Reset fill value for next cooking
            isCooking = false;                  // Reset value since not cooking anymore
            ServeDish();                        // Spawn food at next available location
            recipeActivator.enabled = true;     // Reset collider so player can click again to open recipe menu
        }

        progressBarTransform.GetComponent<Image>().fillAmount = currentAmount / 100;
    }

    // Returns true if there is available slot on the counter and keeps track of its idx,
    // Else returns false
    public bool CheckAvailableSlots()
    {
        for (int i = 0; i < spawnFoodAreas.Length; i++)
        {
            if (spawnFoodAreas[i].transform.childCount == 0)
            {
                availableIdx = i;
                Debug.Log("there's space at " + availableIdx + "!");
                return true;
            }
        }
        return false;
    }

    public void Cook(string dishName)
    {
        if (PlayerWithinRange()) {
            isCooking = true;
            dishToCook = dishName;
        }
    }

    // To spawn the dish at the counter once timer is up
    public void ServeDish()
    {
        Debug.Log("Serving the dish: " + dishToCook);

        // spawn the food in the location at spawnFoodAreas[availableIdx]
        Transform serveLocation = spawnFoodAreas[availableIdx].transform;

        GameObject dish = new GameObject();

        if (dishToCook == "eggplant")
        {
            dish = Instantiate(foodPrefabs[0], serveLocation.position, Quaternion.identity);
        }           
        else if (dishToCook == "goldeggplant")
        {
            dish = Instantiate(foodPrefabs[1], serveLocation.position, Quaternion.identity);
        }
        else if (dishToCook == "pizza")
        {
            dish = Instantiate(foodPrefabs[2], serveLocation.position, Quaternion.identity);
        }
        else if (dishToCook == "burrito")
        {
            dish = Instantiate(foodPrefabs[3], serveLocation.position, Quaternion.identity);
        }

        dish.transform.SetParent(serveLocation);

        dishToCook = ""; // Reset dishToCook value
    }

    bool PlayerWithinRange()
    {
        return Vector3.Distance(playerController.transform.position,
                                stationLocation.position) <= minimumDistance;
    }
}
