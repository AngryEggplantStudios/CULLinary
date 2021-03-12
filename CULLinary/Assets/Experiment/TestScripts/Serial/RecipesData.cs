using UnityEngine;

[System.Serializable]
public class RecipesData
{
    public RecipeTest[] recipes;
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
