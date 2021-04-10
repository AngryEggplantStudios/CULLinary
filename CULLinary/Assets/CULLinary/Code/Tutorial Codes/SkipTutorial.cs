using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class SkipTutorial : MonoBehaviour
{
    public CookingStation cookingStation;
    public DungeonPlayerController dungeonController;

    public GameObject confirmLeaveNotifPanel;
    public GameObject confirmLeaveButton;
    public GameObject[] leaveNotifButtons;
    
    int currIdx = 0;
    int currSceneIdx = 50; //arbitrary value

    [Header("Controls")]
    public string upKey;
    public string downKey;

    private void Start()
    {
        currSceneIdx = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(upKey))
        {
            if (confirmLeaveNotifPanel.activeSelf == true)
                LeavePanelKeyUp();
        }
        if (Input.GetKeyDown(downKey))
        {
            if (confirmLeaveNotifPanel.activeSelf == true)
                LeavePanelKeyDown();
        }
    }

    // NOTIF: Leave scene if player confirms skip tutorial
    public void LeaveTutorial()
    {
        PlayerData playerData = SaveSystem.LoadData();
        playerData.SetRightCustomersServed(1);
        playerData.SetMoney(100);
        SaveSystem.SaveData(playerData);
        SceneManager.LoadScene((int)SceneIndexes.TUT_BOSS);
    }

    public void ShowConfirmLeaveNotifPanel()
    {
        if (currSceneIdx == (int)SceneIndexes.TUT_DUNGEON) // tutorial dungeon scenes 
        {
            dungeonController.DisableMovement();
            confirmLeaveNotifPanel.SetActive(true);  
        }
        else // tutorial restaurant scene
        {
            cookingStation.DisableMovementOfPlayer();
            confirmLeaveNotifPanel.SetActive(true);
        }

        // Auto select cancel option when notif is first shown
        EventSystem.current.SetSelectedGameObject(null); // clear selected object
        EventSystem.current.SetSelectedGameObject(leaveNotifButtons[currIdx]); //set a new selected object
    }

    public void CloseConfirmLeaveNotifPanel()
    {
        if (currSceneIdx == (int)SceneIndexes.TUT_DUNGEON) // tutorial dungeon scenes
        {
            dungeonController.EnableMovement();
            confirmLeaveNotifPanel.SetActive(false);
        }
        else // tutorial restaurant scene
        {         
            cookingStation.EnableMovementOfPlayer();
            confirmLeaveNotifPanel.SetActive(false);
        }        
        
    }

    void LeavePanelKeyUp()
    {
        currIdx--;
        if (currIdx == -1)
            currIdx = 1; //loop back to the last option

        EventSystem.current.SetSelectedGameObject(null); // clear selected object
        EventSystem.current.SetSelectedGameObject(leaveNotifButtons[currIdx]); //set a new selected object
    }
    void LeavePanelKeyDown()
    {
        currIdx++;
        if (currIdx == 2)
            currIdx = 0; //loop back to the first option

        EventSystem.current.SetSelectedGameObject(null); // clear selected object
        EventSystem.current.SetSelectedGameObject(leaveNotifButtons[currIdx]); //set a new selected object
    }
}
