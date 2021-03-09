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
        Debug.Log("Selected: Fried Eggplant");
        menuCanvas.SetActive(false); // close the menu 
        cookingStation.Cook(); // start cooking 
    }

    public void SelectGoldEggplant()
    {
        Debug.Log("Selected: Golden Fried Eggplant");
        menuCanvas.SetActive(false); // close the menu 
        cookingStation.Cook(); // start cooking 
    }

    public void SelectPizza()
    {
        Debug.Log("Selected: Pizza");
        menuCanvas.SetActive(false); // close the menu 
        cookingStation.Cook(); // start cooking 
    }

    public void SelectBurrito()
    {
        Debug.Log("Selected: Burrito");
        menuCanvas.SetActive(false); // close the menu 
        cookingStation.Cook(); // start cooking 
    }

}
