using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{

	new public string name = "New Item";    // Name of the item
	public Sprite icon = null;              // Item icon
	public bool showInInventory = true;
	public int itemId = 0;
	private int itemNo = -1;

	public void SetItemNo(int n)
	{
		itemNo = n;
	}

    public int GetItemNo()
    {
        return itemNo;
    }
}