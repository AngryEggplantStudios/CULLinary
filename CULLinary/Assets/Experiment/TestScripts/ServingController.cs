using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingController : MonoBehaviour
{
    public GameObject[] foodOnCounter; // Can edit later to make this dynamic as more food gets cooked
    public GameObject player;
    public Camera cam;

    public GameObject[] foodPrefabs;

    public bool holdingItem = false;
    public Transform foodLocation;

    // Update is called once per frame
    void Update()
    {
        // if food gets clicked
        // shift the food to player's hands
        
        if (Input.GetMouseButton(0))
        {
            
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                
                if (hit.collider != null && hit.collider.gameObject.tag == "FoodToServe")
                {
                    string selectedDish = hit.collider.gameObject.name;
                    if (selectedDish == ("plateDinner") && !holdingItem)
                    {
                        Debug.Log("Picked up plate");
                        CollectFood();
                    }
                    else if (selectedDish == ("bowlBroth") && !holdingItem)
                    {
                        Debug.Log("Picked up bowl");
                    }
                }
            }
        }
    }

    void CollectFood()
    {
        holdingItem = true;

        // shift food so player looks like carrying the food
        GameObject plateDish = Instantiate(foodPrefabs[0], foodLocation.position, Quaternion.identity);
        plateDish.transform.SetParent(player.transform);

        Destroy(foodOnCounter[0]);
        // player.GetComponent<Animator>().SetBool("PickUpFood", true); // carrying animation
    }

}
