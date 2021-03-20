using UnityEngine;
public class SaveGameDataSystem : MonoBehaviour
{
    private PlayerManager playerManager;
    [SerializeField] private InventoryUI inventoryUI;

    private bool FindPlayerManager()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager != null)
        {
            return true;
        }
        else
        {
            Debug.Log("Cannot find player manager");
            return false;
        }
    }

    public void SaveGameData(int index)
    {
        if (FindPlayerManager() && inventoryUI != null)
        {
            playerManager.SetCurrentIndex(index);
            playerManager.SaveData(inventoryUI.GetItemList());
        }
    }
}