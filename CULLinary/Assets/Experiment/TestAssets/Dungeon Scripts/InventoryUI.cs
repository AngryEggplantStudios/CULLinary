using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	public GameObject inventoryUI;  // The entire UI
	public Transform inventoryPanel;   // The parent object of all the items
	private bool isShowing = false;

	DungeonPlayerInventory inventory;  

	InventorySlot[] slots;

	void Start()
	{
		inventory = DungeonPlayerInventory.instance;
		inventory.onItemChangedCallback += UpdateUI;

		slots = inventoryPanel.GetComponentsInChildren<InventorySlot>();

		inventoryUI.SetActive(false);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			isShowing = !isShowing;
			inventoryUI.SetActive(isShowing);
		}
	}

	// Update the inventory UI by:
	//		- Adding items
	//		- Clearing empty slots
	// This is called using a delegate on the Inventory.
	public void UpdateUI()
	{

		for (int i = 0; i < slots.Length; i++)
		{
			List < Item > itemList = inventory.getItems();
			if (i < itemList.Count)
			{
				slots[i].AddItem(itemList[i]);
			}
			else
			{
				slots[i].ClearSlot();
			}
		}
	}
}
