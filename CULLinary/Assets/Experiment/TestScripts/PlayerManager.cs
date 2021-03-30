using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] private GameData gameData;
    [SerializeField] private GameObject inventoryUI;
    private List<Item> itemList = new List<Item>(); //Inventory
    private int stage; //Current Stage
    private int currentIndex; //Current index
    private string playerName; //Player name
    private int money; //Player amount
    private int maxHealth;

    public static PlayerManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public int GetStage()
    {
        return stage;
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public int GetMoney()
    {
        return money;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetCurrentIndex(int index)
    {
        this.currentIndex = index;
    }

    public void SetStage(int stage)
    {
        this.stage = stage;
    }

    public void SetMoney(int amt)
    {
        this.money = amt;
    }

    public void SetItemList(List<Item> items)
    {
        this.itemList = items;
    }

    public void SetMaxHealth(int h)
    {
        this.maxHealth = h;
    }

    public void SaveData(List<Item> items)
    {
        SetItemList(items);
        string inventory = SerializeInventory();
        PlayerData playerData = new PlayerData(inventory, stage, currentIndex, playerName, money, maxHealth);
        SaveSystem.SaveData(playerData);
    }

    public void SaveData()
    {
        string inventory = SerializeInventory();
        PlayerData playerData = new PlayerData(inventory, stage, currentIndex, playerName, money, maxHealth);
        SaveSystem.SaveData(playerData);
    }

    public void LoadData()
    {
        PlayerData data = SaveSystem.LoadData();
        if (data == null)
        {
            Debug.Log("Data cannot be found. Setting game data to default.");
            SetCurrentIndex(1);
            SetStage(1);
            SetMoney(0);
            SetMaxHealth(150);
            return;
        }
        SetCurrentIndex(data.currentIndex);
        SetStage(data.stage);
        SetMoney(data.money);
        SetMaxHealth(data.maxHealth);
        InventoryItemData[] inventory = JsonArrayParser.FromJson<InventoryItemData>(data.inventory);
        itemList.Clear();
        foreach (InventoryItemData item in inventory)
        {
            for (int i=0; i < item.count; i++)
            {
                itemList.Add(gameData.GetItemById(item.id));
            }
        }
        StartCoroutine(PopulateUI());
    }

    private IEnumerator PopulateUI()
    {
        yield return null;
        InventoryUI inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.PopulateUI(itemList);
        }
    }

    public void InstantiateInventory()
    {
        Instantiate(inventoryUI);
    }

    private string SerializeInventory()
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
