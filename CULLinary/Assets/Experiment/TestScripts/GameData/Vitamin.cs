using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Vitamin", menuName = "Shop/Vitamin")]
public class Vitamin : ShopItem
{
	public int vitaminId;
	public int healthBonus;
	public int meleeAttackBonus;
	public int rangeAttackBonus;
	public int GetHealthBonus()
	{
		return this.healthBonus;
	}
	public int GetRangeAttackBonus()
	{
		return this.rangeAttackBonus;
	}
	public int GetMeleeAttackBonus()
	{
		return this.meleeAttackBonus;
	}
	public int GetID()
	{
		return this.vitaminId;
	}
}