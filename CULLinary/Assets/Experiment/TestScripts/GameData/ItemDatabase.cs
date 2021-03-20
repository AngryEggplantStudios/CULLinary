using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Item Database", menuName="Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> allItems;
}
