using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CookingStation : MonoBehaviour
{
    public Restaurant_newPlayerController playerController;
    public CookingProgressbar cookingProgressBar;
    public Transform stationLocation;
    public float minimumDistance = 7.0f;
    public Animator animator;

    public bool isCooking = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooking && !PlayerWithinRange())
        {
            isCooking = false;
        }

        animator.SetBool("isCooking", isCooking);    
    }

    public void Cook()
    {
        if (PlayerWithinRange()) {
            isCooking = true;
            cookingProgressBar.cookingNow = true;
        }
    }

    bool PlayerWithinRange()
    {
        return Vector3.Distance(playerController.transform.position,
                                stationLocation.position) <= minimumDistance;
    }
}
