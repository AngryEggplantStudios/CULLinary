using UnityEngine;

[System.Serializable]
public class RecipeTest
{
    public int id;
    public string name;
    public int[] ingredientIds;

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
            Debug.Log("No recipes to load. Please check your files.");
        }
    }
}