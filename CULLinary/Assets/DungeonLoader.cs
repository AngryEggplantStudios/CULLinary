using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonLoader : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToHide;
    [SerializeField] private GameObject parentReference;
    [SerializeField] private float delayStartTime = 1f;
    [SerializeField] private Text textToChange;
    [SerializeField] private MapGeneratorNew mapGen;

    public static bool isDoneLoading = false;

    private void Awake()
    {
        isDoneLoading = false;
        foreach(GameObject g in objectsToHide)
        {
            g.SetActive(false);
        }
    }
    private void Update()
    {
        if (MapGeneratorNew.isGenerated)
        {
            StartCoroutine(DelayStart());
        }

        if (MapGeneratorNew.isGeneratingRooms)
        {
            textToChange.text = "Generating Rooms..." + Mathf.RoundToInt(MapGeneratorNew.roomProgress * 100).ToString() + "%";
        }
        else if (MapGeneratorNew.isBuildingNavMesh)
        {
            textToChange.text = "Building the NavMesh...";
        }
        /* else if (MapGeneratorNew.isGeneratingDeadends)
        {
            textToChange.text = "Generating Deadends...";
        } */
        else
        {
            textToChange.text = "Loading Game...";
        }
        
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(delayStartTime);
        foreach(GameObject g in objectsToHide)
        {
            g.SetActive(true);
        }
        isDoneLoading = true;
        yield return null;
        Destroy(parentReference);
    }
}
