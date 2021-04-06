using UnityEngine;
public class SaveGameDataSystem : MonoBehaviour
{
    private PlayerManager playerManager;
    [SerializeField] private InventoryUI inventoryUI;
    private void Start()
    {
        playerManager = PlayerManager.instance;
    }

    public void SaveGameData(int index)
    {
        if (inventoryUI == null)
        {
            inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
        }
        if (inventoryUI != null && playerManager != null)
        {
            PlayerManager.playerData.SetCurrentIndex(index);
            PlayerManager.SaveData(inventoryUI.GetItemList());
        }
    }
}