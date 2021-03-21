using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData: MonoBehaviour
{
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private RecipeDatabase recipeDatabase;
    
    private Dictionary<int, Item> itemDict;
    private List<Item> itemList = new List<Item>();
    private List<Recipe> recipeList = new List<Recipe>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        itemDict = new Dictionary<int, Item>();
        itemList = itemDatabase.allItems;
        recipeList = recipeDatabase.allRecipes;
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

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public List<Recipe> GetRecipeList()
    {
        return recipeList;
    }

    public Dictionary<int, Item> GetItemDict()
    {
        return itemDict;
    }

}
