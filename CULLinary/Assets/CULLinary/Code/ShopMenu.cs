using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [Header("Setting up UI")]
    [SerializeField] private GameObject secondaryCanvas;
    [SerializeField] private GameObject confirmPopup;
    [SerializeField] private GameObject noMoneyPopup;
    [Header("Calculations")]
    [SerializeField] private Text moneyText;
    [SerializeField] private Text currentText;
    [SerializeField] private Text itemCostText;
    [SerializeField] private Text cashLeftText;
    [SerializeField] private Text selectedItemName;
    [SerializeField] private Text selectedItemDesc;
    [SerializeField] private Button yesButton;
    [SerializeField] private Image spriteImage;

    [Header("UI to update")]
    [SerializeField] private CurrentStats currentStats;
    [SerializeField] private GameObject vitaminPanel;
    [SerializeField] private GameObject weaponPanel;
    [SerializeField] private GameObject keyItemPanel;
    private Vitamin currentVitaminSelected;
    private Weapon currentWeaponSelected;
    private KeyItem currentKeyItemSelected;
    private PopulateShop populateShop;
    private int currentPanelSelected = 0; //0=VITAMIN, 1=WEAPON, 2=KEYITEMS

    private void Start()
    {
        populateShop = GetComponentInChildren<PopulateShop>();
        if (PlayerManager.playerData != null)
        {
            moneyText.text = "Money: $" + PlayerManager.playerData.GetMoney();
        }
        currentStats.UpdateUI();
    }
    public void LoadDungeon()
    {
        PlayerManager.playerData.SetCurrentIndex((int)SceneIndexes.DUNGEON);
        PlayerManager.SaveData();
        SceneManager.LoadScene((int)SceneIndexes.DUNGEON);
    }

    public void SetAllPanelsActive()
    {
        vitaminPanel.SetActive(true);
        weaponPanel.SetActive(true);
        keyItemPanel.SetActive(true);
    }

    public void SelectVitaminPanel()
    {
        currentPanelSelected = 0;   
        vitaminPanel.SetActive(true);
        weaponPanel.SetActive(false);
        keyItemPanel.SetActive(false);
    }

    public void SelectWeaponPanel()
    {
        currentPanelSelected = 1;
        vitaminPanel.SetActive(false);
        weaponPanel.SetActive(true);
        keyItemPanel.SetActive(false);
    }

    public void SelectKeyItemPanel()
    {
        currentPanelSelected = 2;
        vitaminPanel.SetActive(false);
        weaponPanel.SetActive(false);
        keyItemPanel.SetActive(true);
    }

    public void SelectNoMoney()
    {
        secondaryCanvas.SetActive(true);
        noMoneyPopup.SetActive(true);
        noMoneyPopup.GetComponentInChildren<Text>().text = "You do not have enough money for this!";
    }

    public void SelectAlreadyBought()
    {
        secondaryCanvas.SetActive(true);
        noMoneyPopup.SetActive(true);
        noMoneyPopup.GetComponentInChildren<Text>().text = "You already bought this!";
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

    public void SelectItem(KeyItem itemAsset)
    {
        SelectItem(itemAsset.price, itemAsset.name, itemAsset.description, itemAsset.GetSprite());
        currentKeyItemSelected = itemAsset;
        yesButton.onClick.AddListener(() => { ConfirmPurchase(itemAsset); });
    }

    public void SelectItem(Weapon itemAsset)
    {
        SelectItem(itemAsset.price, itemAsset.name, itemAsset.description, itemAsset.GetSprite());
        currentWeaponSelected = itemAsset;
        yesButton.onClick.AddListener(() => { ConfirmPurchase(itemAsset); });
    }

    public void ConfirmPurchase(KeyItem itemAsset)
    {
        PlayerManager.playerData.SetMoney(PlayerManager.playerData.GetMoney() - itemAsset.GetPrice());
        PlayerManager.playerData.SetKeyItemBoughtById(itemAsset.GetID(), true);
        yesButton.onClick.RemoveAllListeners();
        DeselectItem();
        UpdateUI();
    }

    public void ConfirmPurchase(Weapon itemAsset)
    {
        PlayerManager.playerData.SetMoney(PlayerManager.playerData.GetMoney() - itemAsset.GetPrice());
        PlayerManager.playerData.SetCritRate(itemAsset.critRate);
        PlayerManager.playerData.SetWeaponBoughtById(itemAsset.GetID(), true);
        yesButton.onClick.RemoveAllListeners();
        DeselectItem();
        UpdateUI();
    }

    public void ConfirmPurchase(Vitamin itemAsset)
    {
        PlayerManager.playerData.SetMaxHealth(PlayerManager.playerData.GetMaxHealth() + currentVitaminSelected.healthBonus);
        PlayerManager.playerData.SetRangeDamage(PlayerManager.playerData.GetRangeDamage() + currentVitaminSelected.rangeAttackBonus);
        PlayerManager.playerData.SetMeleeDamage(PlayerManager.playerData.GetMeleeDamage() + currentVitaminSelected.meleeAttackBonus);
        PlayerManager.playerData.SetCurrentHealth(PlayerManager.playerData.GetCurrentHealth() + currentVitaminSelected.healthHeal);
        PlayerManager.playerData.SetMoney(PlayerManager.playerData.GetMoney() - itemAsset.GetPrice());
        yesButton.onClick.RemoveAllListeners();
        DeselectItem();
        UpdateUI();
    }

    public void DeselectItem()
    {
        secondaryCanvas.SetActive(false);
        confirmPopup.SetActive(false);
        currentVitaminSelected = null;
        currentWeaponSelected = null;
    }

    private void UpdateUI()
    {
        moneyText.text = "Money: $" + PlayerManager.playerData.GetMoney();
        currentStats.UpdateUI();
        StartCoroutine(populateShop.UpdateUI(currentPanelSelected));
    }
}
