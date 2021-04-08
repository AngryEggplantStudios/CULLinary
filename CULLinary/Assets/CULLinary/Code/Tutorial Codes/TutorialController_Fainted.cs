using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class TutorialController_Fainted : MonoBehaviour
{
    public GameObject[] instructionTriggers;
    public TutorialManager TutorialManager;
    public Animator textAnimator;
    public GameObject clownerCust;
    public Transform portalTransform;
    public DialogueLoader dialogueLoader;
    // CookingStation to disable movement when speaking to ClownerCust
    public CookingStation movementController;

    public AudioMixer audio;          // to fade sounds
    public GameObject blackscreen;
    public float originalVolume = 1.0f;
    public float silenceVolume = -0.0001f;
    
    [Tooltip("For save system")]
    public SceneIndexes sceneIndex;
    public SaveGameDataSystem saveGameDataSystem;
    public bool enableGameSave;

    private string channel = "Master_Vol";

    private IEnumerator LoadRealGame()
    {
        float duration = 2.5f;
        StartCoroutine(AudioHelper.FadeAudio(audio, "Master_Vol", duration));
        blackscreen.SetActive(true);
        Animator blackscreenAnimator = blackscreen.GetComponent<Animator>();
        blackscreenAnimator.SetBool("TurnBlack", true); // Fade to black

        yield return new WaitForSeconds(duration); // Have some time for player to process everything before loading the game scene

        // Initiate the real game!
        saveGameDataSystem.SaveGameData((int)SceneIndexes.REST);
        SceneManager.LoadScene((int)SceneIndexes.REST);
    }

    private IEnumerator WaitForInstructions()
    {
        while (textAnimator.GetBool("isOpen")) {
            yield return null;
        }
        // Ended the tutorial
        StartCoroutine(LoadRealGame());
    }

    private IEnumerator EndClowner()
    {
        float duration = 1.0f; // 1 second to rotate
        float currentTime = 0.0f;
        while (currentTime < duration) {
            currentTime = currentTime + Time.deltaTime;
            Vector3 vectorToPortal = portalTransform.position - clownerCust.transform.position;
            Vector3 vectorToPortalWithoutY = new Vector3(vectorToPortal.x,
                                                         0.0f,
                                                         vectorToPortal.z);
            Quaternion newRotation = Quaternion.LookRotation(vectorToPortalWithoutY);
            clownerCust.transform.rotation =
                Quaternion.Slerp(clownerCust.transform.rotation,
                                 newRotation,
                                 currentTime / duration);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        clownerCust.SetActive(false); // leave restaurant

        yield return new WaitForSeconds(1.0f);
        instructionTriggers[0].GetComponent<InstructionTrigger>().TriggerInstruction();
        StartCoroutine(WaitForInstructions());
    }
    
    private IEnumerator PlayMistakenDialogue()
    {
        movementController.DisableMovementOfPlayer();
        yield return new WaitForSeconds(1.0f);

        Dialogue chefDialogueClown = DialogueParser.Parse(
            "{[L]0}No, I meant..." +
            "{[L]0}The clown attacked..." +
            "{[R]1}Uhh... What?" +
            "{[R]1}Take it easy, man. Do you need to see the doctor?" +
            "{[L]0}I feel fine..." +
            "{[R]1}Well, have a good day then." +
            "{[R]1}I've gotta go now. I'm late for my psychiatrist appointment!" +
            "{[R]1}You sure you don't wanna come along?" +
            "{[L]0}Nope, I'm alright. Right as rain!" +
            "{[R]1}Well... See ya later then!");
        dialogueLoader.LoadAndRunWithoutCustomer(chefDialogueClown);
        dialogueLoader.SetDialogueEndCallback(() => StartCoroutine(EndClowner()));
    }

    private IEnumerator PlayClownExplanationDialogue()
    {
        movementController.DisableMovementOfPlayer();
        yield return new WaitForSeconds(1.0f);

        Dialogue chefDialogueClown = DialogueParser.Parse(
            "{[L]0}Wait..." +
            "{[L]0}What about the clown?" +
            "{[R]1}Clown?" +
            "{[R]1}Oh, I was saying..." +
            "{[R]1}The clown is behind all the chaos." +
            "{[R]1}All the fruits and veggies are under his control." +
            "{[R]1}Apparently he was a normal guy who was in charge of the factory..." +
            "{[R]1}He must have been exposed to the chemicals as well.");
        dialogueLoader.LoadAndRunWithoutCustomer(chefDialogueClown);
        dialogueLoader.SetDialogueEndCallback(() => StartCoroutine(PlayMistakenDialogue()));
    }

    private IEnumerator PlayChefDialogue()
    {
        movementController.DisableMovementOfPlayer();

        Animator blackscreenAnimator = blackscreen.GetComponent<Animator>();
        blackscreenAnimator.SetBool("GetRidOfBlack", true); // Fade from black
        
        float duration = 1.0f; // 1 second to fade
        float currentTime = 0.0f;
        while (currentTime < duration) {
            currentTime = currentTime + Time.deltaTime;
            float newVolume = Mathf.Lerp(silenceVolume, originalVolume, currentTime / duration);
            audio.SetFloat(channel, Mathf.Log10(newVolume) * 20);
            yield return null;
        }

        Dialogue chefDialogue = DialogueParser.Parse(
            "{[L]0}Ugggh..." +
            "{[L]0}My head... " +
            "{[R]1}AAHHH!! You're alive! Jeepers!" +
            "{[L]0}What happened?" +
            "{[R]1}There was an earthquake! Everyone ran away." +
            "{[R]1}I stayed in the restaurant." +
            "{[R]1}When it was over, you were lying on the floor.");
        dialogueLoader.LoadAndRunWithoutCustomer(chefDialogue);
        dialogueLoader.SetDialogueEndCallback(() => StartCoroutine(PlayClownExplanationDialogue()));
    }

    private void PlayCustomerDialogue()
    {
        movementController.DisableMovementOfPlayer();
        Dialogue clownerDialogue = DialogueParser.Parse(
            "{[R]1}Are you okay?" +
            "{[R]1}Oh no..." +
            "{[R]1}Let's see... *takes out phone*" +
            "{[R]1}What... to... do... when... person... unresponsive...");
        dialogueLoader.LoadAndRunWithoutCustomer(clownerDialogue);
        dialogueLoader.SetDialogueEndCallback(() => StartCoroutine(PlayChefDialogue()));
    }

    private IEnumerator StartWelcome()
    {
        movementController.DisableMovementOfPlayer();
        yield return new WaitForSeconds(2.0f); // Allow scene to load before triggering scripted messages
        PlayCustomerDialogue();
    }

    private IEnumerator SaveGame()
    {
        yield return new WaitForSeconds(0.5f);
        if (enableGameSave)
        {
            // save at Tut_Fainted
            saveGameDataSystem.SaveGameData((int)sceneIndex);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        audio.SetFloat(channel, Mathf.Log10(silenceVolume) * 20);
        StartCoroutine(SaveGame());
        StartCoroutine(StartWelcome());
    }
}
