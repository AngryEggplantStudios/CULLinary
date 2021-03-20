using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// In charge of controlling when to show Menu UI and other notifications
public class UIController : MonoBehaviour
{
    public CookingStation cookingStation;

    private int totalAmt = 0;

    [Header("UI Elements")]
    public GameObject moneyText;
    public GameObject InventoryPanel;
    public GameObject MenuPanel;
    public GameObject CounterNotifPanel;

    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = PlayerManager.instance;
        if (playerManager != null)
        {
            totalAmt = playerManager.GetMoney();
            moneyText.GetComponent<Text>().text = "Amount earned: $" + totalAmt.ToString();
        }
    }

    // For Inventory Panel and Menu Panel (Called by CookingStation)
    public void ShowCookingPanel()
    {
        InventoryPanel.SetActive(true);
        MenuPanel.SetActive(true);
    }

    public void CloseCookingPanel()
    {
        InventoryPanel.SetActive(false);
        MenuPanel.SetActive(false);
    }

    // NOTIF: "Not enough counter space"
    public void ShowCounterNotifPanel()
    {
        InventoryPanel.SetActive(false);
        MenuPanel.SetActive(false);
        CounterNotifPanel.SetActive(true);
    }

    public void CloseCounterNotifPanel()
    {
        CounterNotifPanel.SetActive(false);
        cookingStation.EnableMovementOfPlayer(); // Enable player movement so they can serve food when receive notif that counter has no space
    }

    // To update the Amount Earned at top left hand corner
    public void AddWrongDishEarnings()
    {
        totalAmt += 50;
        moneyText.GetComponent<Text>().text = "Amount earned: $" + totalAmt.ToString();
        AddToGameData();
    }

    public void AddCorrectDishEarnings()
    {
        totalAmt += 100;
        moneyText.GetComponent<Text>().text = "Amount earned: $" + totalAmt.ToString();
        AddToGameData();
    }

    private void AddToGameData()
    {
        
        if (playerManager != null)
        {
            playerManager.SetMoney(totalAmt);
        }
    }
}
