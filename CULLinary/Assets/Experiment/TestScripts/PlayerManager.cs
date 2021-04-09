using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    private static List<Item> itemList = new List<Item>(); //Inventory
    public static PlayerData playerData;
    public static PlayerManager instance;

    public static float currHealth;
    public static int noOfMobsCulled;
    public static int wrongCustomersServed;
    public static int rightCustomersServed;

    public static float gameTime;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    public static void SaveData(List<Item> items)
    {
        itemList = items;
        playerData.SetCurrentHealth((int)Mathf.Min(Mathf.Max(100, currHealth), playerData.maxHealth));
        playerData.SetNoOfMobsCulled(noOfMobsCulled);
        playerData.SetRightCustomersServed(rightCustomersServed);
        playerData.SetWrongCustomersServed(wrongCustomersServed);
        playerData.SetInventoryString(SerializeInventory(itemList));
        SaveGameTime();
        SaveSystem.SaveData(playerData);
    }

    public static void SaveDataTutorial(List<Item> items)
    {
        itemList = items;
        playerData.SetInventoryString(SerializeInventory(itemList));
        SaveSystem.SaveData(playerData);
    }

    public static void SaveDataShop()
    {
        playerData.SetInventoryString(SerializeInventory(itemList));
        SaveGameTime();
        SaveSystem.SaveData(playerData);
    }

    public static void SaveDataBoss()
    {
        playerData.SetCurrentIndex((int)SceneIndexes.REST);
        playerData.SetCurrentHealth((int)Mathf.Min(Mathf.Max(100, currHealth), playerData.maxHealth));
        playerData.SetInventoryString(SerializeInventory(itemList));
        SaveGameTime();
        SaveBossTime();
        SaveSystem.SaveData(playerData);
    }

    public static void SaveGameTime()
    {
        playerData.SetGameTime(GameTimer.GetTime() + playerData.GetGameTime());
    }

    public static void SaveBossTime()
    {
        playerData.SetBossTime(BossTimer.GetTime());
    }

    public static void LoadData()
    {
        playerData = SaveSystem.LoadData();
        currHealth = playerData.GetCurrentHealth();
        noOfMobsCulled = playerData.GetNoOfMobsCulled();
        wrongCustomersServed = playerData.GetWrongCustomersServed();
        rightCustomersServed = playerData.GetRightCustomersServed();
        if (playerData == null)
        {
            playerData = new PlayerData();
            return;
        }
        InventoryItemData[] inventory = JsonArrayParser.FromJson<InventoryItemData>(playerData.GetInventoryString());
        itemList.Clear();
        foreach (InventoryItemData item in inventory)
        {
            for (int i=0; i < item.count; i++)
            {
                itemList.Add(GameData.GetItemById(item.id));
            }
        }
        instance.StartCoroutine(PopulateUI());
        
    }
    private static IEnumerator PopulateUI()
    {
        yield return new WaitForSeconds(0.5f);
        InventoryUI inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.PopulateUI(itemList);
        }
    }

    private static string SerializeInventory(List<Item> itemList)
    {
        Dictionary<int, int> inventory = new Dictionary<int, int>();
        
        foreach (Item item in itemList)
        {
            if (inventory.ContainsKey(item.itemId))
            {
                inventory[item.itemId] += 1;
            }
            else
            {
                inventory.Add(item.itemId, 1);
            }
        }
        InventoryItemData[] items = new InventoryItemData[inventory.Count];
        int i = 0;
        foreach (var item in inventory)
        {
            InventoryItemData gameItem = new InventoryItemData(item.Key, item.Value);
            items[i] = gameItem;
            i++;
        }
        return JsonArrayParser.ToJson(items, true);
    }

}
