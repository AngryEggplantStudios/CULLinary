using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class ShopItem : ScriptableObject
{
	new public string name = "New Shop Item";
	public Sprite icon = null;
	public string description = "";
	public int price;
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

    public Sprite GetSprite()
    {
        return this.icon;
    }
}