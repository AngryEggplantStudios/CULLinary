using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController_Dun : MonoBehaviour
{
    public GameObject[] instructionTriggers;

    public TutorialManager TutorialManager;
    public GameObject Enemies;
    public GameObject inventoryPanel;
    public GameObject recipePanel;

    bool playerMoved = false;
    bool triedAttacking = false;
    bool killedOne = false;
    bool killedAll = false;
    bool checkedInventory = false;
    bool checkedRecipes = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartWelcome");
    }

    IEnumerator StartWelcome()
    {
        yield return new WaitForSeconds(1); // Allow scene to load before triggering welcome msg

        instructionTriggers[0].GetComponent<InstructionTrigger>().TriggerInstruction();
    }

    private void Update()
    {
        if (triedAttacking == false)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) // Show next sentence if player presses mouse keys
            {
                StartCoroutine(AdvanceInstructions());
                triedAttacking = true;
                // Debug.Log("Player attacked");
            }               
        }

        if ((playerMoved == false) && (triedAttacking == true))
        {
            if ((Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d"))) // Show next sentence if player presses wasd keys
            {
                StartCoroutine(AdvanceInstructions());
                playerMoved = true;
                // Debug.Log("Player moved");
            }
        }

        if (killedOne == false)
        {
            if (Enemies.transform.childCount == 2) // Killed one eggplant
            {
                killedOne = true;
                Debug.Log("Killed one eggplant!");
                instructionTriggers[1].GetComponent<InstructionTrigger>().TriggerInstruction();
            }
        }
        if (killedAll == false)
        {
            if (Enemies.transform.childCount == 0) // Killed all 3 eggplants
            {
                killedAll = true;
                Debug.Log("Killed all eggplants!");
                instructionTriggers[2].GetComponent<InstructionTrigger>().TriggerInstruction();
            }
        }

        if (checkedInventory == false)
        {
            if (Input.GetKeyDown("i")) // Show next sentence if player presses I key
            {
                StartCoroutine(AdvanceInstructions());
                checkedInventory = true;
            }
        }

        if ((checkedRecipes == false) && (checkedInventory == true))
        {
            if (Input.GetKeyDown("r")) // Show next sentence if player presses R key
            {
                StartCoroutine(AdvanceInstructions());
                checkedRecipes = true;

                StartCoroutine(HideMenus());
            }
        }

    }

    IEnumerator AdvanceInstructions()
    {
        yield return new WaitForSeconds(2); // Allow players to try out move before next sentence shows

        TutorialManager.DisplayNextSentence();
    }

    IEnumerator HideMenus()
    {
        yield return new WaitForSeconds(5); // Allow players to try out move before next sentence shows

        inventoryPanel.SetActive(false);
        recipePanel.SetActive(false);
    }

}
