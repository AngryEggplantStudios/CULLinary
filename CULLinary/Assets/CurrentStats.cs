using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentStats : MonoBehaviour
{
    [SerializeField] private Text healthValue;
    [SerializeField] private Text rangeValue;
    [SerializeField] private Text meleeValue;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        healthValue.text = PlayerManager.playerData.currentHealth + "/" + PlayerManager.playerData.GetMaxHealth();
        rangeValue.text = PlayerManager.playerData.GetRangeDamage().ToString();
        meleeValue.text = PlayerManager.playerData.GetMeleeDamage().ToString();
    }
}
