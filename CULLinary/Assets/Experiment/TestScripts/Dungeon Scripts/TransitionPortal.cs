using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPortal : MonoBehaviour
{
    [SerializeField] private SceneIndexes sceneIndex;
    [SerializeField] private SaveGameDataSystem saveGameDataSystem;
    [SerializeField] private bool enableGameSave;
    
    private bool isTriggered; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isTriggered)
        {
            
            bool showWarningMsg = false;

            // If in restaurant scene, check for any current food before transiting
            Scene currScene = SceneManager.GetActiveScene();
            
            if (currScene.name == "TestRestaurant")
            {

                //Check if any leftover food
                CookingStation cookingStation = GameObject.Find("Recipe Controller").GetComponent<CookingStation>();
                GameObject[] spawnFoodAreas = cookingStation.spawnFoodAreas;
                for (int i = 0; i < spawnFoodAreas.Length; i++)
                {
                    if (spawnFoodAreas[i].transform.childCount != 0)
                    {
                        showWarningMsg = true;
                        break;
                    }
                }

                // If there if leftover food, show warning message
                // Else, let player transit
                if (showWarningMsg)
                {
                    UIController uiController = GameObject.Find("UI Controller").GetComponent<UIController>();
                    uiController.ShowConfirmLeaveNotifPanel();
                } else
                {
                    if (enableGameSave)
                    {
                        saveGameDataSystem.SaveGameData((int)sceneIndex);
                    }
                    LoadScene();
                }                   
            }
            else // in dungeon
            {
                if (enableGameSave)
                {
                    saveGameDataSystem.SaveGameData((int)sceneIndex);
                }
                LoadScene();
            }
            /*
            Debug.Log("Player can transit");
            if (enableGameSave)
            {
                saveGameDataSystem.SaveGameData(sceneIndex);
            }
            LoadScene();
            */
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene((int)sceneIndex);
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

    public void ConfirmLeaveRestaurant()
    {
        Debug.Log("Player can transit");
        if (enableGameSave)
        {
            saveGameDataSystem.SaveGameData((int)sceneIndex);
        }
        LoadScene();
    }
}
