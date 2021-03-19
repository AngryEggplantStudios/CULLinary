using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Deals with any of the buttons that can be clicked in the Menu Panel
public class RecipeSelector : MonoBehaviour
{
    public CookingStation cookingStation;
    public UIController uiController;

    // Closes the UI panel 
    public void SelectCloseButton()
    {
        uiController.CloseCookingPanel();
    }

    // Closes the Menu panel and cooks food if there is enough space on the counter
    private void StartCooking(string dishName)
    {
        if (cookingStation.CheckAvailableSlots() == true)
        {
            Debug.Log("Selected: " + dishName);
            uiController.CloseCookingPanel(); // close the menu 
            cookingStation.Cook(dishName); // start cooking
        } else
        {
            uiController.ShowCounterNotifPanel(); //show UI notif that counter has no more space to place food
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
