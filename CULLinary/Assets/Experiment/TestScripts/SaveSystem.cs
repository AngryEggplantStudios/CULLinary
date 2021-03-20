using UnityEngine;

public static class SaveSystem
{
    public static void SaveData(PlayerData playerData)
    {

        FileManager.WriteToFile("saveFile.clown", playerData.ToJson());
    }

    public static PlayerData LoadData()
    {
        if (FileManager.LoadFromFile("saveFile.clown", out var json))
        {
            PlayerData playerData = new PlayerData();
            playerData.LoadFromJson(json);
            return playerData;
        }
        Debug.Log("Save file not loaded");
        return null;

    }

    public static void CreateNewFile(PlayerData playerData)
    {
        
        FileManager.WriteToFile("saveFile.clown", playerData.ToJson()); //Default save name
    }
}

/*
--- Old Implementation of using Binary Formatters
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

SaveData
BinaryFormatter formatter = new BinaryFormatter();
FileStream stream = new FileStream(path, FileMode.Create);
PlayerData data = new PlayerData(playerManager.GetItemList(), playerManager.GetStage());
formatter.Serialize(stream, data);
stream.Close();
        
LoadData
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
*/