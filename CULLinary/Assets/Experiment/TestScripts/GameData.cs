using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData: MonoBehaviour
{
    [SerializeField] private ItemDatabase itemDatabase;
    
    private Dictionary<int, Item> itemDict;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        itemDict = new Dictionary<int, Item>();
        StartCoroutine(PopulateItemDatabase());
    }

    private IEnumerator PopulateItemDatabase()
    {
        foreach(Item i in itemDatabase.allItems)
        {
            try
            {
                itemDict.Add(i.itemId, i);
            }
            catch
            {
                Debug.Log("Unable to add item: " + i.name);
            }
            yield return null;
        }
    }

    public Item GetItemById(int id)
    {
        return itemDict[id];
    }

}
