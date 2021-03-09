using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSelector : MonoBehaviour
{
    // public Button eggplantButton;
    // public Button goldEggplantButton;
    // public Button pizzaButton;
    // public Button burritoButton;

    // To carry out the cooking stuffs
    // Just going to print the dish's name for now
    public void SelectEggplant()
    {
        Debug.Log("Selected: Fried Eggplant");
    }

    public void SelectGoldEggplant()
    {
        Debug.Log("Selected: Golden Fried Eggplant");
    }

    public void SelectPizza()
    {
        Debug.Log("Selected: Pizza");
    }

    public void SelectBurrito()
    {
        Debug.Log("Selected: Burrito");
    }

}
