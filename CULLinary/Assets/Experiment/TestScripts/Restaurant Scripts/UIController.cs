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

    private void Start()
    {
        //StartCoroutine(FindInventoryPanel());
    }

    private IEnumerator FindInventoryPanel()
    {
        while (true)
        {
            this.InventoryPanel = GameObject.FindGameObjectWithTag("InventoryPanel");
            if (this.InventoryPanel != null)
            {
                this.InventoryPanel.SetActive(false);
                break;
            }
            yield return new WaitForSeconds(0.1f);
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
    }

    public void AddCorrectDishEarnings()
    {
        totalAmt += 100;
        moneyText.GetComponent<Text>().text = "Amount earned: $" + totalAmt.ToString();
    }
}
