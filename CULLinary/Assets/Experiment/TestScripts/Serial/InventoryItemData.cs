[System.Serializable]
public class InventoryItemData
{
    public int id;
    public int count;

    public InventoryItemData(int id, int count)
    {
        this.id = id;
        this.count = count;
    }
}