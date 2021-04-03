using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// KEEPS TRACK OF MAX CUST SO WE KNOW WHEN THE FIRST FELLA LEAVES
public class Return_CustCounter : MonoBehaviour
{
    public GameObject[] PossibleSeats; // populate all the seats
    public TutorialController_Return returnController;

    int maxCustNum = 0;
    int currCustNum = 0;
    bool firstCustLeft = false;

    // Update is called once per frame
    void Update()
    {
        if (firstCustLeft == false)
        {
            currCustNum = 0;

            for (int i = 0; i < PossibleSeats.Length; i++)
            {
                if (PossibleSeats[i].transform.childCount != 0) // if there is a customer sitting
                {
                    currCustNum++;
                }
            }

            if (currCustNum < maxCustNum)
            {
                returnController.ShowLeaveTutorialMsg(); // trigger the final message if a customer just left (ie. the curr cound != as our max count)
                firstCustLeft = true;
            }
            else
            {
                maxCustNum = currCustNum;
            }
        }           
    }
}
