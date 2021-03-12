using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [SerializeField] private TextAsset recipesFile;
    [SerializeField] private TextAsset ingredientsFile;
    public static RecipeTest[] recipes;
    public static Ingredient[] ingredients;
    private void Awake()
    {
        LoadRecipeData();
        LoadIngredientData();
    }

    private RecipeTest[] LoadRecipeData()
    {
        RecipesData r = JsonUtility.FromJson<RecipesData>(recipesFile.text);
        recipes = r.recipes;
        return r.recipes;
    }

    private Ingredient[] LoadIngredientData()
    {
        IngredientsData i = JsonUtility.FromJson<IngredientsData>(ingredientsFile.text);
        ingredients = i.ingredients;
        return i.ingredients;
    }
    
}
