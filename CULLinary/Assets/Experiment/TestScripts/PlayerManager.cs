using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{

    private List<Item> itemList = new List<Item>(); //Inventory
    private int stage; //Current Stage
    private int currentIndex; //Current index
    private string playerName; //Player name
    private int money; //Player amount

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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

    public void SetCurrentIndex(int index)
    {
        this.currentIndex = index;
    }

    public void SetMoney(int amt)
    {
        this.money = amt;
    }

    public void SetItemList(List<Item> items)
    {
        this.itemList = items;
    }

    public void SaveData(List<Item> items)
    {
        SetItemList(items);
        string inventory = SerializeInventory();
        PlayerData playerData = new PlayerData(inventory, stage, currentIndex, playerName);
        SaveSystem.SaveData(playerData);
    }

    public void LoadData()
    {
        PlayerData data = SaveSystem.LoadData();
        if (data == null)
        {
            Debug.Log("Data cannot be found.");
            return;
        }

        stage = data.stage;
        playerName = data.playerName;
        money = data.money;
        currentIndex = data.currentIndex;
        InventoryItemData[] inventory = JsonArrayParser.FromJson<InventoryItemData>(data.inventory);
        foreach (var item in inventory)
        {
            //Debug.Log(item.id);
            //Debug.Log(item.count);
        }
        Debug.Log(stage);
        Debug.Log(playerName);
        Debug.Log(money);
        Debug.Log(currentIndex);
    }

    private void ParseInventory()
    {
        Debug.Log("parsing in progress");
    }

    private string SerializeInventory()
    {
        Dictionary<int, int> inventory = new Dictionary<int, int>();
        
        foreach (var item in itemList)
        {
            if (inventory.ContainsKey(item.GetItemNo()))
            {
                inventory[item.GetItemNo()] += 1;
            }
            else
            {
                inventory.Add(item.GetItemNo(), 1);
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
