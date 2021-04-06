using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinaleController : MonoBehaviour
{
    public Animator blackscreenAnimator;
    public Animator rollingCreditsAnimator;

    // Start is called before the first frame update
    void Start()
    {
        // ShowCredits(); 
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

        StartCoroutine(GoBackRestaurant());
    }

    IEnumerator GoBackRestaurant()
    {
        yield return new WaitForSeconds(20);

        SceneManager.LoadScene((int)SceneIndexes.REST); // Load restaurant scene 
    }
}
