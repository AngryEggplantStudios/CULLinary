using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject secondaryCanvas;
    [SerializeField] private GameObject confirmPopup;

    public void LoadDungeon()
    {
        Debug.Log("Saving file");
        Debug.Log("Loading Dungeon...");
    }

    public void SelectItem(GameObject itemAsset)
    {
        secondaryCanvas.SetActive(true);
        confirmPopup.SetActive(true);
    }

    public void DeselectItem()
    {
        secondaryCanvas.SetActive(false);
        confirmPopup.SetActive(false);
    }

}
