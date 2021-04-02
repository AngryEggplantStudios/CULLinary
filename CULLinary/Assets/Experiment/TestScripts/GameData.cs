using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData: MonoBehaviour
{
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private RecipeDatabase recipeDatabase;
    
    private static Dictionary<int, Item> itemDict;
    private static List<Item> itemList = new List<Item>();
    private static List<Recipe> recipeList = new List<Recipe>();

    public static GameData instance;

    private void Awake()
    {
        instance = this;
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

    public static Item GetItemById(int id)
    {
        return itemDict[id];
    }

    public static List<Item> GetItemList()
    {
        return itemList;
    }

    public static List<Recipe> GetRecipeList()
    {
        return recipeList;
    }

    public static Dictionary<int, Item> GetItemDict()
    {
        return itemDict;
    }

}
