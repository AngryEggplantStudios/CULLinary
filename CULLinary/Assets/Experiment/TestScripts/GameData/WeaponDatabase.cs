using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Weapon Database", menuName="Shop/WeaponDatabase")]
public class WeaponDatabase : ScriptableObject
{
    public List<Weapon> allWeapons;
}
