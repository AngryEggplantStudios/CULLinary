using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSelector : MonoBehaviour
{
    public CookingStation cookingStation;
    public GameObject menuCanvas;

    // To carry out the cooking stuffs
    // Just going to print the dish's name for now
    public void SelectEggplant()
    {
        if (cookingStation.CheckAvailableSlots() == true)
        {
            Debug.Log("Selected: Fried Eggplant");
            menuCanvas.SetActive(false); // close the menu 
            cookingStation.Cook("eggplant"); // start cooking
        }       
    }

    public void SelectGoldEggplant()
    {
        if (cookingStation.CheckAvailableSlots() == true)
        {
            Debug.Log("Selected: Golden Fried Eggplant");
            menuCanvas.SetActive(false); // close the menu 
            cookingStation.Cook("goldeggplant"); // start cooking 
        }
       
    }

    public void SelectPizza()
    {
        if (cookingStation.CheckAvailableSlots() == true)
        {
            Debug.Log("Selected: Pizza");
            menuCanvas.SetActive(false); // close the menu 
            cookingStation.Cook("pizza"); // start cooking 
        }
       
    }

    public void SelectBurrito()
    {
        if (cookingStation.CheckAvailableSlots() == true)
        {
            Debug.Log("Selected: Burrito");
            menuCanvas.SetActive(false); // close the menu 
            cookingStation.Cook("burrito"); // start cooking 
        }         
    }

}
