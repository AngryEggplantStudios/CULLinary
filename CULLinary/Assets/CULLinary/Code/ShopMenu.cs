using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject secondaryCanvas;
    [SerializeField] private GameObject confirmPopup;
    [SerializeField] private GameObject noMoneyPopup;
    [SerializeField] private Text moneyText;
    [SerializeField] private Text currentText;
    [SerializeField] private Text itemCostText;
    [SerializeField] private Text cashLeftText;
    [SerializeField] private Text selectedItemName;
    [SerializeField] private Text selectedItemDesc;
    [SerializeField] private Button yesButton;
    [SerializeField] private Image spriteImage;

    [SerializeField] private CurrentStats currentStats;
    private Vitamin currentVitaminSelected;

    private void Start()
    {
        UpdateUI();
    }
    public void LoadDungeon()
    {
        PlayerManager.playerData.SetCurrentIndex((int)SceneIndexes.DUNGEON);
        PlayerManager.SaveData();
        SceneManager.LoadScene((int)SceneIndexes.DUNGEON);
    }

    public void SelectNoMoney()
    {
        secondaryCanvas.SetActive(true);
        noMoneyPopup.SetActive(true);
    }

    public void BackToShop()
    {
        secondaryCanvas.SetActive(false);
        noMoneyPopup.SetActive(false);
    }

    public void SelectItem(int price, string name, string desc, Sprite img)
    {
        secondaryCanvas.SetActive(true);
        confirmPopup.SetActive(true);
        currentText.text = "$" + PlayerManager.playerData.GetMoney();
        itemCostText.text = "- $" + price;
        cashLeftText.text = "$" + (PlayerManager.playerData.GetMoney() - price);
        selectedItemName.text = name;
        selectedItemDesc.text = desc;
        spriteImage.sprite = img;
    }

    public void SelectItem(Vitamin itemAsset)
    {
        SelectItem(itemAsset.price, itemAsset.name, itemAsset.description, itemAsset.GetSprite());
        currentVitaminSelected = itemAsset;
        yesButton.onClick.AddListener(() => { ConfirmPurchase(itemAsset); });
    }

    public void ConfirmPurchase(Vitamin itemAsset)
    {
        DeselectItem();
        PlayerManager.playerData.SetMaxHealth(PlayerManager.playerData.GetMaxHealth() + currentVitaminSelected.healthBonus);
        PlayerManager.playerData.SetRangeDamage(PlayerManager.playerData.GetRangeDamage() + currentVitaminSelected.rangeAttackBonus);
        PlayerManager.playerData.SetMoney(PlayerManager.playerData.GetMoney() - itemAsset.GetPrice());
        yesButton.onClick.RemoveAllListeners();
        UpdateUI();
    }

    public void DeselectItem()
    {
        secondaryCanvas.SetActive(false);
        confirmPopup.SetActive(false);
    }

    private void UpdateUI()
    {
        moneyText.text = "Money: $" + PlayerManager.playerData.GetMoney();
        currentStats.UpdateUI();
    }
}
