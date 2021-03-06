using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(PlayerData playerData)
    {
        /*
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(playerManager.GetItemList(), playerManager.GetStage());
        formatter.Serialize(stream, data);
        stream.Close();
        */
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
        /*
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

    }
}
