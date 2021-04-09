using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsLoader : MonoBehaviour
{
    [SerializeField] private Text money;
    [SerializeField] private Text health;
    [SerializeField] private Text ranged;
    [SerializeField] private Text melee;
    [SerializeField] private Text crit;
    [SerializeField] private Text noOfMobsCulled;
    [SerializeField] private Text customersServed;

    private void OnEnable()
    {
        money.text = "Money: $" + PlayerManager.playerData.money;
        health.text = "Health: " + (int)PlayerManager.currHealth + "/" + PlayerManager.playerData.maxHealth;
        ranged.text = "Ranged: " + PlayerManager.playerData.rangeDamage + "dmg";
        melee.text = "Melee: " + PlayerManager.playerData.meleeDamage + "dmg";
        crit.text = "Crit Rate: " + PlayerManager.playerData.critRate + "%";
        noOfMobsCulled.text = "Culled: " + PlayerManager.noOfMobsCulled + " mobs";
        customersServed.text = "Served: " + (PlayerManager.wrongCustomersServed + PlayerManager.rightCustomersServed) + " customers";
    }
}
