using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour
{
	[SerializeField] private GameObject inventoryUI;  // The entire UI
	[SerializeField] private Transform inventoryPanel;   // The parent object of all the items
	private bool isShowing = false;

	private DungeonPlayerInventory inventory;

	[SerializeField] private InventorySlot[] slots;
	private List<Item> itemList = new List<Item>(); // Inventory
	[SerializeField] private int inventoryLimit = 16;
	
    [SerializeField] private GameObject damageCounter_prefab;

    public static InventoryUI instance;

    private void Awake()
    {
        instance = this;
    }
	private void Start()
	{
		slots = inventoryPanel.GetComponentsInChildren<InventorySlot>();
        Debug.Log(slots);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			ToggleVisiblity();
		}
	}

	public void ToggleVisiblity()
	{
		isShowing = !isShowing;
		inventoryUI.SetActive(isShowing);
	}

	public void AddItem(Item item)
	{
        
		if (itemList.Count < inventoryLimit)
		{
			itemList.Add(item);
			UpdateUI();
		}
		else
		{
			Debug.Log("Not enough room");
		}

	}

    public void PopulateUI(List<Item> items)
    {
        itemList = items;
        UpdateUI();
    }

	public void RemoveItem(Item item)
	{
		itemList.Remove(item);
		UpdateUI();
	}

    public List<Item> GetItemList()
    {
        return itemList;
    }

	// Update the inventory UI by:
	//		- Adding items
	//		- Clearing empty slots
	// This is called using a delegate on the Inventory.
	private void UpdateUI()
	{
        if (slots != null)
        {
            for (int i = 0; i < slots.Length; i++)
            {
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
}
