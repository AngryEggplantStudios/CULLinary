using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerInventory : MonoBehaviour
{
    private bool showInventory;
    private List<Item> itemList = new List<Item>();
    public static DungeonPlayerInventory instance;
    private Loot currentCollidedItem;

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
        Debug.Log("Item added");
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
            AddItemIntoInventory(currentCollidedItem);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Loot loot = other.GetComponent<Loot>();
        if (loot != null)
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
