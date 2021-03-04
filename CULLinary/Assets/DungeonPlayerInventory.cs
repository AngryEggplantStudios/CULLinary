using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerInventory : MonoBehaviour
{
    private bool showInventory;
    private List<Item> itemList = new List<Item>();
    public static DungeonPlayerInventory instance;
    private Loot currentCollidedItem; 
    public int space = 20;	// Amount of item spaces

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    void Awake()
    {
        instance = this;
    }

    public void AddItemIntoInventory(Loot loot)
    {
        itemList.Add(loot.getItem());
        loot.pickUp();
        currentCollidedItem = null;
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void Remove(Item item)
    {
        itemList.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public List<Item> getItems()
    {
        return this.itemList;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentCollidedItem != null)
        {
            if (itemList.Count >= space)
            {
                Debug.Log("Not enough room.");
                return;
            }

            AddItemIntoInventory(currentCollidedItem);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Loot loot = other.GetComponent<Loot>();
        if (loot != null && currentCollidedItem == null)
        {
            currentCollidedItem = loot;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Loot loot = other.GetComponent<Loot>();
        if (loot != null && currentCollidedItem == null)
        {
            currentCollidedItem = loot;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Loot loot = other.GetComponent<Loot>();
        if (loot != null)
        {
            currentCollidedItem = loot;
        }
    }
}
