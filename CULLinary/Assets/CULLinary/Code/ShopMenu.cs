using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject secondaryCanvas;
    [SerializeField] private GameObject confirmPopup;

    private Vitamin currentItem;

    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = PlayerManager.instance;
    }

    public void LoadDungeon()
    {
        //Debug.Log(playerManager.GetMaxHealth());
        Debug.Log("Saving file");
        PlayerManager.SaveData();
        Debug.Log("Loading Dungeon...");
        SceneManager.LoadScene(2);
    }

    //Only works for vitamins now
    public void SelectItem(Vitamin itemAsset)
    {
        secondaryCanvas.SetActive(true);
        confirmPopup.SetActive(true);
        currentItem = itemAsset;
    }

    public void ConfirmPurchase()
    {
        DeselectItem();
        PlayerManager.playerData.SetMaxHealth(PlayerManager.playerData.GetMaxHealth() + currentItem.healthBonus);
    }

    public void DeselectItem()
    {
        secondaryCanvas.SetActive(false);
        confirmPopup.SetActive(false);
    }

    

}
