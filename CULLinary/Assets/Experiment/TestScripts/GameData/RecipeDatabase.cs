using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New KeyItem Database", menuName="Shop/KeyItemDatabase")]
public class KeyItemDatabase : ScriptableObject
{
    public List<KeyItem> allKeyItems;
}
