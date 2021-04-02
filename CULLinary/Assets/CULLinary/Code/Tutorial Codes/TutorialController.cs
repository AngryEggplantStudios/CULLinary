using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject[] instructionTriggers;
    public GameObject PossibleSeats;
    public GameObject CustomerSpawner;
    public UIController UIController;
    public GameObject MenuPanel;

    bool firstCustArrived = false;
    bool playerMoved = false;
    bool triedCooking = false;

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
        if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d"))
        {
            playerMoved = true;

            CustomerSpawner.SetActive(true); // Start spawning customers once player tries moving around
        }

        if (playerMoved && (firstCustArrived == false))
        {
            foreach (Transform seat in PossibleSeats.transform)
            {
                if (seat.childCount == 1) // our first customer arrived
                {
                    FirstCustomerArrivedMsg();
                    firstCustArrived = true;
                }
            }   
        }

        if ((MenuPanel.activeSelf == true) && (triedCooking == false))
        {
            instructionTriggers[2].GetComponent<InstructionTrigger>().TriggerInstruction();
            triedCooking = true;
        }
    }

    public void FirstCustomerArrivedMsg()
    {
        instructionTriggers[1].GetComponent<InstructionTrigger>().TriggerInstruction();
    }

}
