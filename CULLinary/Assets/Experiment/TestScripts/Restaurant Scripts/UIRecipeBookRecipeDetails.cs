using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This should match the Recipe struct used in the list of recipes
public class UIRecipeBookRecipeDetails : MonoBehaviour
{
    public Text text;
    public GameObject recipeEntry;
    public GameObject imagePrefab;

    // Temporary, to be replaced with item database
    // Item sprite should be located at the array
    // index that corresponds to the item ID
    public Sprite[] itemSprites;

    private string recipeName;
    private int recipeId;
    private int[] ingredientIds;

    public void SetRecipe(RecipeTest rec)
    {
        this.ingredientIds = rec.getIngredients();
        this.recipeName = rec.getName();
        this.recipeId = rec.getId();
        
        text.text = recipeName;
		foreach (int id in ingredientIds) {
            GameObject itemRepresentation = Instantiate(imagePrefab,
                                                        new Vector3(0, 0, 0),
                                                        Quaternion.identity,
                                                        recipeEntry.transform) as GameObject;

            Image itemSprite = itemRepresentation.GetComponent<Image>();
            itemSprite.sprite = itemSprites[id];
        }
    }
}
