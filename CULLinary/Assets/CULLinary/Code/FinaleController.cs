using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class FinaleController : MonoBehaviour
{
    public Animator blackscreenAnimator;
    public Animator rollingCreditsAnimator;
    public GameObject creditsCanvas;

    public GameObject clownerCust;
    public DialogueLoader dialogueLoader;
    public Restaurant_CustomerController customerController;   
    public CookingStation movementController; // CookingStation to disable movement when speaking to ClownerCust

    public AudioMixer audio; // to fade sounds

    private float creditsDuration = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        // ShowCredits(); 
        StartCoroutine("SpawnFinalCust");
    }

    IEnumerator SpawnFinalCust()
    {
        yield return new WaitForSeconds(2); // can adjust this longer/shorter as you deem fit

        clownerCust.SetActive(true);
        TutFindPlayer findPlayerAi = clownerCust.GetComponent<TutFindPlayer>();

        if (findPlayerAi)
        {
            findPlayerAi.SetReachedPlayerCallback(() => {
                movementController.DisableMovementOfPlayer();
                Dialogue clownerDialogue = DialogueParser.Parse(
                    "{[R]1}Hey chef, thanks for the happy meal-" +
                    "{[R]1}Knowing the monsters are gone really puts a smile to our faces :)" +
                    "{[R]1}Thank you for restoring peace to the town!");
                dialogueLoader.LoadAndRun(clownerDialogue, customerController);
                dialogueLoader.SetDialogueEndCallback(() => {
                    Debug.Log("Rolling the credits ~");
                    movementController.DisableMovementOfPlayer();
                    ShowCredits(); // not sure if should change this to coroutine?
                });
            });
        }
    }

    // Call this method to start showing the Credits
    void ShowCredits()
    {
        creditsCanvas.SetActive(true);
        blackscreenAnimator.SetBool("TurnBlack", true); // Fade credits black bg
        StartCoroutine(RollCredits());
    }

    IEnumerator RollCredits()
    {
        yield return new WaitForSeconds(1.5f);

        rollingCreditsAnimator.SetBool("LetsRoll", true); // Roll da credits

        StartCoroutine(GoBackMenu()); // initially GoBackRestaurant() but i think loading back to main menu makes more sense??
    }

    IEnumerator GoBackRestaurant()
    {
        yield return new WaitForSeconds(creditsDuration);

        SceneManager.LoadScene((int)SceneIndexes.REST); // Load restaurant scene 
    }

    IEnumerator GoBackMenu()
    {
        float fadeDuration = 1.5f;
        yield return new WaitForSeconds(creditsDuration - fadeDuration);

        StartCoroutine(AudioHelper.FadeAudio(audio, "Master_Vol", fadeDuration));
        yield return new WaitForSeconds(fadeDuration);

        PlayerPrefs.SetInt("Just_Won_Game", 1);             // Trigger victory music in main menu
        SceneManager.LoadScene((int)SceneIndexes.MAINMENU); // Load main menu scene 
    }
}
