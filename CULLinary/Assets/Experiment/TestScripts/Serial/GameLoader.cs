using UnityEngine;
using System;
using System.IO;
public static class GameLoader
{
    public static RecipesData LoadRecipes()
    {
        string currentFilePath = Directory.GetCurrentDirectory() + "/Assets/Experiment/TestScripts/Serial/Recipes.json";
        Debug.Log(currentFilePath);
        if (FileManager.LoadFromFile(currentFilePath, out var json, true))
        {
            Debug.Log("Application Path is " + Application.dataPath);
            RecipesData recipes = new RecipesData();
            recipes.LoadFromJson(json);
            Debug.Log(recipes.recipes);
            foreach (RecipeTest r in recipes.recipes)
            {
                Debug.Log(r.name);
            }
            return recipes;
        }
        Debug.Log("Game data not found!");
        return null;
    }
}