using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private Item itemForLoot;

    public Item getItem()
    {
        return itemForLoot;
    }

    public void pickUp()
    {
        Destroy(gameObject);
    }
}
