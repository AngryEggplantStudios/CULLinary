using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController_Return : MonoBehaviour
{
    public GameObject[] instructionTriggers;
    public GameObject SpawnFoodLocation;
    public TutorialManager TutorialManager;
    public GameObject PossibleSeats;
    public Animator textAnimator;

    bool cookedDish = false;
    bool pickedUpDish = false;
    bool firstCustArrived = false;
    bool firstCustLeft = false;

    GameObject firstCustSeat = null;

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
        if (cookedDish == false)
        {
            if (SpawnFoodLocation.transform.childCount != 0) // food cooked and on counter
            {
                instructionTriggers[1].GetComponent<InstructionTrigger>().TriggerInstruction();
                cookedDish = true;
                StartCoroutine(AdvanceInstructions()); // Auto advance for this particular instruction
            }          
        }

        if (pickedUpDish == false)
        {
            if ( (SpawnFoodLocation.transform.childCount == 0) && (cookedDish == true) ) // food cooked and on counter
            {
                instructionTriggers[2].GetComponent<InstructionTrigger>().TriggerInstruction();
                pickedUpDish = true;
                StartCoroutine(AdvanceInstructions()); // Auto advance for this particular instruction
            }
        }

        if (firstCustArrived == false)
        {
            foreach (Transform seat in PossibleSeats.transform)
            {
                if (seat.childCount == 1) // our first customer arrived
                {
                    firstCustSeat = seat.gameObject; 
                    firstCustArrived = true;
                }
            }
        }
        if ((firstCustArrived == true) && (firstCustLeft == false))
        {
            if (firstCustSeat.transform.childCount == 0) // our first customer left
            {
                instructionTriggers[3].GetComponent<InstructionTrigger>().TriggerInstruction();
                firstCustLeft = true;
            }    
        }
        
        if ((firstCustLeft == true) && (textAnimator.GetBool("isOpen") == false)) // once instruction textbox goes away
        {
            SceneManager.LoadScene(1); // or 2 - let them start from dungeon?
        }
    }

    IEnumerator AdvanceInstructions()
    {
        yield return new WaitForSeconds(2); // Allow players to try out move before next sentence shows

        TutorialManager.DisplayNextSentence();
    }
}
