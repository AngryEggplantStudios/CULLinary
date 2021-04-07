using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinaleController : MonoBehaviour
{
    public Animator blackscreenAnimator;
    public Animator rollingCreditsAnimator;

    public GameObject clownerCust;
    public DialogueLoader dialogueLoader;
    public Restaurant_CustomerController customerController;   
    public CookingStation movementController; // CookingStation to disable movement when speaking to ClownerCust

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
                    ShowCredits(); // not sure if should change this to coroutine?
                });
            });
        }
    }

    // Call this method to start showing the Credits
    void ShowCredits()
    {
        blackscreenAnimator.SetBool("TurnBlack", true); // Fade credits black bg

        StartCoroutine(RollCredits());
    }

    IEnumerator RollCredits()
    {
        yield return new WaitForSeconds(1.5f);

        rollingCreditsAnimator.SetBool("LetsRoll", true); // Roll da credits
        
        yield return new WaitForSeconds(1.5f);

        StartCoroutine(GoBackMenu()); // initially GoBackRestaurant() but i think loading back to main menu makes more sense??
    }

    IEnumerator GoBackRestaurant()
    {
        yield return new WaitForSeconds(27);

        SceneManager.LoadScene((int)SceneIndexes.REST); // Load restaurant scene 
    }

    IEnumerator GoBackMenu()
    {
        yield return new WaitForSeconds(27);

        SceneManager.LoadScene((int)SceneIndexes.MAINMENU); // Load restaurant scene 
    }
}
