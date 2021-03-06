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
}
