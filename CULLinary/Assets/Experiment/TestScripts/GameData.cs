using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [SerializeField] private TextAsset recipesFile;
    private void Start()
    {
        RecipesData r = JsonUtility.FromJson<RecipesData>(recipesFile.text);
        Debug.Log(r.recipes);
        foreach (RecipeTest rec in r.recipes)
        {
            Debug.Log(rec.id + ", " + rec.name + ", " + rec.ingredientIds[0]);
        }
    }
}
