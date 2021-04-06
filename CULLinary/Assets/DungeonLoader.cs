using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonLoader : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSetActive;
    [SerializeField] private Text textToChange;

    public static bool isDoneLoading = false;

    private void Update()
    {
        if (MapGeneratorNew.isGeneratingRooms)
        {
            textToChange.text = "Generating Rooms..." + Mathf.RoundToInt(MapGeneratorNew.roomProgress * 100).ToString() + "%";
        }
        else if (MapGeneratorNew.isBuildingNavMesh)
        {
            textToChange.text = "Building the NavMesh...";
        }
        else if (MapGeneratorNew.isLoadingGame)
        {
            textToChange.text = "Loading in Player...";
        }
        else
        {
            isDoneLoading = true;
            foreach (GameObject g in objectsToSetActive)
            {
                g.SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
