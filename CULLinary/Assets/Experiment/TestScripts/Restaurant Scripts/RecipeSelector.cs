using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Deals with any of the buttons that can be clicked in the Menu Panel
public class RecipeSelector : MonoBehaviour
{
    public CookingStation cookingStation;
    public GameObject menuCanvas;

    // Closes the UI panel 
    public void SelectCloseButton()
    {
        menuCanvas.SetActive(false);
    }

    // Closes the Menu panel and cooks food if there is enough space on the counter
    private void StartCooking(string dishName)
    {
        if (cookingStation.CheckAvailableSlots() == true)
        {
            Debug.Log("Selected: " + dishName);
            menuCanvas.SetActive(false); // close the menu 
            cookingStation.Cook(dishName); // start cooking
        } else
        {
            // Can we show a notif here that counter has no more space to place food(?)
            Debug.Log("No more space on counter!!");
        }
    }

    // Methods to trigger when player clicks on the relevant button
    // May need to abstract out more if we intend to spawn new recipe buttons (>4 recipes than the ones we currently have) dynamically...? 
    // or maybe just pre-make them and save them as prefabs, then spawn them dynamically? --> KIV first
    public void SelectEggplant()
    {
        StartCooking("eggplant");
    }

    public void SelectGoldEggplant()
    {
        StartCooking("goldeggplant");       
    }

    public void SelectPizza()
    {
        StartCooking("pizza");       
    }

    public void SelectBurrito()
    {
        StartCooking("burrito");
    }

}
