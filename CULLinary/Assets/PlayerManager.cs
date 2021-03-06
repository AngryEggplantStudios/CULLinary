using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [Serializable] public class GameInventoryItem
    {
        public int sno;
        public int num;
    }

    private List<Item> itemList = new List<Item>();
    private int stage = 1;
    private string playerName = "Clown";

    private void Awake()
    {
        LoadData();
    }

    private void Setup()
    {
        LoadList();
        stage++;
    }

    private void LoadList()
    {
        if (inventoryUI != null) 
        {
        itemList.AddRange(inventoryUI.GetComponent<InventoryUI>().GetItemList());
        }
        
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public int GetStage()
    {
        return stage;
    }

    public void SaveData()
    {
        Setup();
        PlayerData playerData = new PlayerData();
        playerData.stage = stage;
        playerData.playerName = playerName;
        playerData.inventory = SerializeInventory();
        Debug.Log(playerData.inventory);
        SaveSystem.SaveData(playerData);
    }

    public void LoadData()
    {
        PlayerData data = SaveSystem.LoadData();
        if (data == null)
        {
            return;
        }

        try 
        {
            stage = data.stage;
            playerName = data.playerName;
            GameInventoryItem[] inventory = JsonArrayParser.FromJson<GameInventoryItem>(data.inventory);
            foreach (var item in inventory)
            {
                Debug.Log(item.sno);
                Debug.Log(item.num);
            }
            Debug.Log(stage);
            Debug.Log(playerName);
        }
        catch
        {
            Debug.Log("File is corrupted");
        }
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
        GameInventoryItem[] items = new GameInventoryItem[inventory.Count];
        int i = 0;
        foreach (var item in inventory)
        {
            GameInventoryItem gameItem = new GameInventoryItem();
            gameItem.sno = item.Key;
            gameItem.num = item.Value;
            items[i] = gameItem;
            i++;
        }
        return JsonArrayParser.ToJson(items, true);
    }
}
