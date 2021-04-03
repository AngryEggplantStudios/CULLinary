using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRecipeBook : MonoBehaviour
{
    // Entire recipe book, to hide or show the recipes
	public GameObject recipeBookScrollView;
    // Container to attach the recipes to
	public GameObject recipeContainer;
    // Prefab of a recipe book entry
    public GameObject recipeBookEntryPrefab;

    private bool isRecipeBookVisible = true;
	public static bool recipeIsLoaded = false;

    // Populate the recipe book
	private void Start()
	{
		recipeIsLoaded = false;
		foreach (RecipeTest rec in Recipes.recipes) {
            GameObject recBookEntry = Instantiate(recipeBookEntryPrefab,
                                                  new Vector3(0, 0, 0),
                                                  Quaternion.identity,
                                                  recipeContainer.transform) as GameObject;
            
            UIRecipeBookRecipeDetails recDetails = recBookEntry.GetComponent<UIRecipeBookRecipeDetails>();
            recDetails.SetRecipe(rec);
        }
        // Hide recipe book at the start
        ToggleVisiblity();
		recipeIsLoaded = true;
	}

    // Use keyboard to toggle the recipe book
	private void Update()
	{
		if (Keybinds.WasTriggered(Keybind.OpenRecipeBook))
		{
		    ToggleVisiblity();
		}
	}

    // Sets the recipe book to be visible or invisible
	public void SetActive(bool isActive)
	{
		isRecipeBookVisible = isActive;
		recipeBookScrollView.SetActive(isActive);
	}

    // Toggle the recipe book to be visible or not
    public void ToggleVisiblity()
	{
		isRecipeBookVisible = !isRecipeBookVisible;
		recipeBookScrollView.SetActive(isRecipeBookVisible);
	}
}
