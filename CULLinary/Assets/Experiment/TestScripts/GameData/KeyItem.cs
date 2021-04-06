using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Key Item", menuName= "Shop/KeyItem")]
public class KeyItem : ShopItem
{
    public int keyItemId;
    
    public int GetID()
    {
        return this.keyItemId;
    }

}
