using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossPortal : MonoBehaviour
{
    [SerializeField] private SaveGameDataSystem saveGameDataSystem;
    [SerializeField] private SceneIndexes bossScene;
    // This field will be assigned by the map generator
    public DialogueLoader dialogueLoader = null;
    
    private bool isTriggered;

    private void TransitionToBossScene()
    {
        if (saveGameDataSystem != null) {
            saveGameDataSystem.SaveGameData((int)SceneIndexes.DUNGEON);
        }
        Debug.Log("Entering boss");
        SceneManager.LoadScene((int)bossScene);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isTriggered)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (dialogueLoader == null || player == null) {
                Debug.Log("Dialogue loader or player not found");
                // should save current health too
                TransitionToBossScene();
            } else {
                DungeonPlayerController dpc = player.GetComponent<DungeonPlayerController>();
                DungeonPlayerAim dpa = player.GetComponent<DungeonPlayerAim>();
                DungeonPlayerSlash dps = player.GetComponent<DungeonPlayerSlash>();
                if (dpc == null || dpa == null || dps == null) {
                    Debug.Log("Player controllers not found. Will not disable player controls");
                } else {
                    dpc.DisableMovement();
                    dpa.disableMovement();
                    dps.disableMovement();  // Re-enabled implictly by loading new scene
                }
                Animator playerAnim = player.GetComponent<Animator>();
                if (playerAnim == null) {
                    Debug.Log("Player animator not found. May get stuck in running animation");
                } else {
                    playerAnim.SetFloat("Speed", 0.0f);
                }
                Dialogue enterPortalDialogue = DialogueParser.Parse(
                    "{[L]0}It smells like fried potatoes and chemicals!" +
                    "Well... here goes nothing.");
                dialogueLoader.LoadAndRunWithoutCustomer(enterPortalDialogue);
                dialogueLoader.SetDialogueEndCallback(() => TransitionToBossScene());
            }
        }
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
