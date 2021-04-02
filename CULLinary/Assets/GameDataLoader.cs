using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataLoader : MonoBehaviour
{
    private void Start()
    {
        if (PlayerManager.instance != null)
        {
            PlayerManager.LoadData();
        }
    }
}
