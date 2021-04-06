using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Vitamin", menuName = "Shop/Vitamin")]
public class Vitamin : ShopItem
{
	public int vitaminId;
	public int healthBonus;
	public int healthHeal;
	public int meleeAttackBonus;
	public int rangeAttackBonus;
	public int GetID()
	{
		return this.vitaminId;
	}
}