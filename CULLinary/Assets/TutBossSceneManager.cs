using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutBossSceneManager : MonoBehaviour
{
    public DungeonPlayerController dungeonController;
    public DungeonPlayerAim dungeonAim;
    public DungeonPlayerSlash dungeonSlash;
    public DialogueLoader dialogueLoader;

    [Tooltip("For save system")]
    public SceneIndexes sceneIndex;
    public SaveGameDataSystem saveGameDataSystem;
    public bool enableGameSave;

    private IEnumerator SaveGame()
    {
        yield return new WaitForSeconds(0.5f);
        if (enableGameSave)
        {
            // save at Tut_Fainted
            PlayerManager.playerData.SetMoney(100);
            PlayerManager.rightCustomersServed = 1;
            saveGameDataSystem.SaveGameData((int)sceneIndex);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SaveGame());
        StartCoroutine("StartBoss");
        dungeonController.DisableMovement();
        //Hacky way of disabling dungeonAim LOL. For now:
        dungeonAim.disableMovement();
        dungeonSlash.disableMovement();
        loadCutsceneDialogue();
    }

    private void loadCutsceneDialogue()
    {
        Dialogue clownerDialogue = DialogueParser.Parse(
        "{[L]0}Wait is that a huge McDo...McRonald Clown??" +
        "{[L]0}Oh no! He spotted me!");
            dialogueLoader.LoadAndRunWithoutCustomer(clownerDialogue);
    }

    IEnumerator StartBoss()
    {
        yield return new WaitForSeconds(1); // Allow scene to load before triggering scene
    }
}
