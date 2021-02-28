using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CookingStation : MonoBehaviour
{
    public UnityEvent cookingEvent;
    public UnityEvent stopCookingEvent;
    public Restaurant_PlayerController playerController;
    public Transform stationLocation;
    public float minimumDistance = 7.0f;

    bool isCooking = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCooking && playerController.clickedCooking &&
            Vector3.Distance(playerController.transform.position,
                             stationLocation.position) <= minimumDistance) {
                
            cookingEvent.Invoke();
            isCooking = true;
        } else if (isCooking &&
                   Vector3.Distance(playerController.transform.position,
                                    stationLocation.position) > minimumDistance) {

            stopCookingEvent.Invoke();
            isCooking = false;
        }
    }
}
