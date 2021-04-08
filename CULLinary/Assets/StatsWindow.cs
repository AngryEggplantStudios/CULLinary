using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsWindow : MonoBehaviour
{
    [SerializeField] private GameObject statsPanel;

    private bool isStatsWindowVisible = false;

    void Update()
    {
        if (Keybinds.WasTriggered(Keybind.StatsMenu) && PlayerManager.instance != null)
        {
            ToggleVisiblity();
        }
    }

    private void ToggleVisiblity()
    {
        isStatsWindowVisible = !isStatsWindowVisible;
        statsPanel.SetActive(isStatsWindowVisible);
    }
}
