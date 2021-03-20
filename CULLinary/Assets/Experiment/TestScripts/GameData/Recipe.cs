using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Restaurant/Recipe")]
public class Recipe : ScriptableObject
{
	new public string name = "New Item";    // Name of the item
	public Sprite icon = null;              // Recipe icon?
	public int recipeId;
	public int[] ingredientList;
}