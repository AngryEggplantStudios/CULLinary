using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In charge of transforming dish's location from counter to player, and then from player to customer
public class ServingController : MonoBehaviour
{
    public GameObject player;
    public Camera cam;

    public GameObject[] foodPrefabs;
    public int foodId;

    public bool holdingItem = false;
    public Transform foodLocation;
    private GameObject currCustomer = null; // to store current customer player is serving

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetMouseButton(0))
        {
            
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Pick up food from counter
                Debug.Log("Clicking on: " + hit.collider.gameObject.name);

                if (hit.collider != null && hit.collider.gameObject.tag == "FoodToServe" && !holdingItem)
                {
                    string selectedDish = hit.collider.gameObject.name;
                    selectedDish = selectedDish.Remove(selectedDish.Length - 7); // Remove the "(Clone)"
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
                    
                    for( int i  = 0; i < foodPrefabs.Length; i++)
                    {
                        if (foodPrefabs[i].name == selectedDish)
                        {
                            foodId = i;
                        }
                    }

                    CollectFood();
                    Destroy(hit.collider.gameObject); // comment this out if want unlimited servings of the dish after cooking
                }

                // Serve customer
                if (hit.collider != null && hit.collider.gameObject.tag == "Customer")
                {
                    // Serve the customer if carrying food
                    Debug.Log("Clicked Customer");
                    if (holdingItem)
                    {
                        Debug.Log("Serving Customer!");
                        currCustomer = hit.collider.gameObject;
                        ServeFood(currCustomer);
                    }
                }
            }
        }
    }

    // Shift food so player looks like carrying the food
    // Remove food from counter accordingly
    void CollectFood()
    {
        holdingItem = true;
   
        GameObject plateDish = Instantiate(foodPrefabs[foodId], foodLocation.position, Quaternion.identity);
        plateDish.transform.SetParent(foodLocation);
    }

    // Shift food so player looks has served food to customer
    // Remove food from player's hands accordingly
    void ServeFood(GameObject customer)
    {
        Transform serveLocation = customer.transform.Find("Serve Food Location");

        GameObject plateDish = Instantiate(foodPrefabs[foodId], serveLocation.position, Quaternion.identity);
        plateDish.transform.SetParent(serveLocation);

        holdingItem = false; // reset bool value since not carrying anything anymore

        var foodLocation = player.transform.Find("FoodLocation"); // Destroy food the player was carrying
        GameObject holdingPos = foodLocation.gameObject;

        foreach (Transform child in holdingPos.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        currCustomer = null;
    }

    }
