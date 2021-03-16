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

public class Recipes
{
    public Recipe[] recipes = new Recipe[]{
        new Recipe("Fried Eggplant", 0, new int[]{0, 2, 3}),
        new Recipe("Golden Fried Eggplant", 1, new int[]{1, 2, 3}),
        new Recipe("Pizza", 2, new int[]{2, 3, 4, 5, 6}),
        new Recipe("Burrito", 3, new int[]{4, 7, 8, 9, 10})
    };
}
