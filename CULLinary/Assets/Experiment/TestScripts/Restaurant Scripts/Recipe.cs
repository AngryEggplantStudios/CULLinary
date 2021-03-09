using System;

public class Recipe
{
    private string name;
    private int recipeId;
    private int[] ingredientIds;

    public Recipe(string nm, int id, int[] ingredients) {
        name = nm;
        recipeId = id;
        ingredientIds = ingredients;
    }

    public string getName() {
        return name;
    }

    public int getId() {
        return recipeId;
    }

    public int[] getIngredients() {
        return ingredientIds;
    }

    public bool checkIfIngredientRequired(int ingredientId) {
        return Array.Exists(ingredientIds, x => x == ingredientId);
    }
}
