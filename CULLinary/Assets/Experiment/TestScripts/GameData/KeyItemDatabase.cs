using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Recipe Database", menuName="Restaurant/RecipeDatabase")]
public class RecipeDatabase : ScriptableObject
{
    public List<Recipe> allRecipes;
}
