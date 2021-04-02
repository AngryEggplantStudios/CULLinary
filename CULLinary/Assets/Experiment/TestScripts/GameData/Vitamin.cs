using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Vitamin", menuName = "Shop/Vitamin")]
public class Vitamin : ScriptableObject
{
	new public string name = "New Vitamin";
	public Sprite icon = null;
	public string description = "";
	public int vitaminId;

	public int price;
	public int healthBonus;
	public int attackBonus;

	public string GetName()
	{
		return this.name;
	}

	public string GetDescription()
	{
		return this.description;
	}

	public int GetPrice()
	{
		return this.price;
	}

	public int GetHealthBonus()
	{
		return this.healthBonus;
	}

	public int GetAttackBonus()
	{
		return this.attackBonus;
	}


}