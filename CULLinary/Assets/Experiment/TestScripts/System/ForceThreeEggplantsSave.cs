using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceThreeEggplantsSave : SaveGameDataSystem
{
    public List<Item> threeEggplants;

    public override void SaveGameData(int index)
    {
        if (playerManager != null)
        {
            PlayerManager.playerData.SetCurrentIndex(index);
            PlayerManager.SaveData(threeEggplants);
        } else {
            Debug.Log("Rip, failed to save the 3 eggplants :(");
        }
    }
}
