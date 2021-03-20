using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string inventory;
    public int stage; //references the game progress in terms of stage
    public int currentIndex; //references whether we are in the rest/dungeon
    public string playerName;
    public int money;

    public PlayerData()
    {
        this.inventory = "";
        this.stage = 0;
        this.money = 0;
        this.currentIndex = 1; //Default goes to restaurant
        this.playerName = "John Doe";
    }

    public PlayerData(string inventory, int stage, int currentIndex, string playerName, int money)
    {
        this.inventory = inventory;
        this.stage = stage;
        this.currentIndex = currentIndex; //Default goes to restaurant
        this.playerName = playerName;
        this.money = money;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string jsonData)
    {
        try
        {
            JsonUtility.FromJsonOverwrite(jsonData, this);
        } 
        catch
        {
            Debug.Log("No save file...");
        }
    }

}