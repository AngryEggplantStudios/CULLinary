using UnityEngine;
public class SaveGameDataSystem : MonoBehaviour
{
    private PlayerManager playerManager;
    [SerializeField] private InventoryUI inventoryUI;
    private bool hasPlayerManager = false;
    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager != null)
        {
            hasPlayerManager = true;
        }
        else
        {
            Debug.Log("Cannot find player manager");
        }
    }

    public void SetCurrentIndex(int index)
    {
        if (hasPlayerManager)
        {
            playerManager.SetCurrentIndex(index);
        }
        
    }

    public void SaveInventoryToPlayerManager()
    {
        if (hasPlayerManager)
        {
            playerManager.SaveData(inventoryUI.GetItemList());
        }
        else
        {
            Debug.Log("Unable to save inventory to player manager");
        }
        
    }
}