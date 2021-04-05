using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "New Weapon", menuName="Shop/Weapon")]
public class Weapon : ShopItem
{
    public int weaponId;
    public bool doubleFire;
    public int GetID()
    {
        return this.weaponId;
    }
    public bool GetDoubleFire()
    {
        return this.doubleFire;
    }
}
