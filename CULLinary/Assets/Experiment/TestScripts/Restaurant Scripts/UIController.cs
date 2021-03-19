using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// In charge of controlling when to show Menu UI and other notifications
public class UIController : MonoBehaviour
{
    public CookingStation cookingStation;

    [Header("UI Elements")]
    public GameObject uiCanvas;
    public GameObject InventoryPanel;
    public GameObject MenuPanel;
    public GameObject CounterNotifPanel;

    // For Inventory Panel and Menu Panel (Called by CookingStation)
    public void ShowCookingPanel()
    {
        uiCanvas.SetActive(true);
        InventoryPanel.SetActive(true);
        MenuPanel.SetActive(true);
    }

    public void CloseCookingPanel()
    {
        InventoryPanel.SetActive(false);
        MenuPanel.SetActive(false);
        uiCanvas.SetActive(false);
    }

    // NOTIF: "Not enough counter space"
    public void ShowCounterNotifPanel()
    {
        uiCanvas.SetActive(true); // for redudancy
        InventoryPanel.SetActive(false);
        MenuPanel.SetActive(false);
        CounterNotifPanel.SetActive(true);
    }

    public void CloseCounterNotifPanel()
    {
        CounterNotifPanel.SetActive(false);
        uiCanvas.SetActive(false);
        cookingStation.EnableMovementOfPlayer(); // Enable player movement so they can serve food when receive notif that counter has no space
    }
}
