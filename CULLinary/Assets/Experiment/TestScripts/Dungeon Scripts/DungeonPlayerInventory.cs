using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerInventory : MonoBehaviour
{
    public static DungeonPlayerInventory instance;
    private Loot currentCollidedItem; 

    public delegate void OnItemChanged(Item item);
    public OnItemChanged OnItemAdd;
    public OnItemChanged OnItemRemove;

    void Awake()
    {
        instance = this;
    }

    public void AddItemIntoInventory(Loot loot)
    {
        Item item = loot.GetItem();
        item.SetItemNo(loot.GetItemNo());
        loot.PickUp();
        currentCollidedItem = null;
        OnItemAdd?.Invoke(item);
    }

    public void Remove(Item item)
    {
        OnItemRemove?.Invoke(item);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentCollidedItem != null)
        {
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
