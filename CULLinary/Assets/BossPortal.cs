using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossPortal : MonoBehaviour
{
    [SerializeField] private SaveGameDataSystem saveGameDataSystem;
    [SerializeField] private SceneIndexes bossScene;
    
    private bool isTriggered;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isTriggered)
        {
            // should save current health too
            Debug.Log("Entering boss");
            saveGameDataSystem.SaveGameData((int)SceneIndexes.DUNGEON);
            LoadScene();
        }
    }
    private void LoadScene()
    {
        SceneManager.LoadScene((int)bossScene);
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            isTriggered = false;
        }
    }
}
