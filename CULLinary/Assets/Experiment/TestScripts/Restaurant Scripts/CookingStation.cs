using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CookingStation : MonoBehaviour
{
    public GameObject[] spawnFoodAreas;
    int availableIdx = 0;

    public GameObject[] foodPrefabs;

    public Restaurant_newPlayerController playerController;
    public CookingProgressbar cookingProgressBar;
    public Transform stationLocation;
    public float minimumDistance = 7.0f;
    public Animator animator;

    public bool isCooking = false;

    string dishToCook = "";

    // Update is called once per frame
    void Update()
    {
        if (isCooking && !PlayerWithinRange())
        {
            isCooking = false;
        }

        animator.SetBool("isCooking", isCooking);    
    }

    public void Cook(string dishName)
    {
        if (PlayerWithinRange()) {
            isCooking = true;
            cookingProgressBar.cookingNow = true;
            dishToCook = dishName;
        }
    }

    public bool CheckAvailableSlots()
    {
        for( int i = 0; i < spawnFoodAreas.Length; i++)
        {
            if (spawnFoodAreas[i].transform.childCount == 0)
            {
                availableIdx = i;
                Debug.Log("there's space at " + availableIdx + "!" );
                return true;
            }
        }

        Debug.Log("no more space to serve"); // not sure if want to add a UI thing to notify player there's no more space on counter
        return false;
    }

    public void ServeDish()
    {
        Debug.Log("Serving the dish: " + dishToCook);

        // spawn the food in the location at spawnFoodAreas[availableIdx]
        Transform serveLocation = spawnFoodAreas[availableIdx].transform;

        if (dishToCook == "eggplant")
        {
            GameObject dish = Instantiate(foodPrefabs[0], serveLocation.position, Quaternion.identity);
            dish.transform.SetParent(serveLocation);
        }           
        else if (dishToCook == "goldeggplant")
        {
            GameObject dish = Instantiate(foodPrefabs[1], serveLocation.position, Quaternion.identity);
            dish.transform.SetParent(serveLocation);
        }
        else if (dishToCook == "pizza")
        {
            GameObject dish = Instantiate(foodPrefabs[2], serveLocation.position, Quaternion.identity);
            dish.transform.SetParent(serveLocation);
        }
        else if (dishToCook == "burrito")
        {
            GameObject dish = Instantiate(foodPrefabs[3], serveLocation.position, Quaternion.identity);
            dish.transform.SetParent(serveLocation);
        }

        dishToCook = ""; // Reset dishToCook value
    }

    bool PlayerWithinRange()
    {
        return Vector3.Distance(playerController.transform.position,
                                stationLocation.position) <= minimumDistance;
    }
}
