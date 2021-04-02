using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Vitamin Database", menuName="Shop/VitaminDatabase")]
public class VitaminDatabase : ScriptableObject
{
    public List<Vitamin> allVitamins;
}

