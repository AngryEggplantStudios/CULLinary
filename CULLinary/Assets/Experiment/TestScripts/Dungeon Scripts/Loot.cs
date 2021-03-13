using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private Item itemForLoot;
    [SerializeField] private int itemNo;

    public Item GetItem()
    {
        return itemForLoot;
    }

    public int GetItemNo()
    {
        return itemNo;
    }

    public void PickUp()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        DungeonPlayerInventory inventory = other.GetComponent<DungeonPlayerInventory>();
        if (inventory != null)
        {
            inventory.AddItemIntoInventory(this);
        }
    }
}
