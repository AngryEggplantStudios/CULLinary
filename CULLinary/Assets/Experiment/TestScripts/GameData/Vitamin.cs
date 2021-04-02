using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Vitamin", menuName = "Shop/Vitamin")]
public class Vitamin : ShopItem
{
	public int vitaminId;
	public int healthBonus;
	public int attackBonus;
	public int GetHealthBonus()
	{
		return this.healthBonus;
	}
	public int GetAttackBonus()
	{
		return this.attackBonus;
	}
	public int GetID()
	{
		return this.vitaminId;
	}
}