using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentStats : MonoBehaviour
{
    [SerializeField] private Text healthValue;
    [SerializeField] private Text rangeValue;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        healthValue.text = PlayerManager.playerData.GetMaxHealth().ToString();
        rangeValue.text = PlayerManager.playerData.GetRangeDamage().ToString();
    }
}
