using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocateCamera : MonoBehaviour
{
    public Transform currentPlayerTransform;

    Vector3 initialPlayerLocation;
    Vector3 initialCameraLocation;

    // Start is called before the first frame update
    void Start()
    {
        initialCameraLocation = transform.position;
        initialPlayerLocation = currentPlayerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPlayerLocation = currentPlayerTransform.position;
        transform.position = new Vector3(
            initialCameraLocation.x + currentPlayerLocation.x - initialPlayerLocation.x,
            initialCameraLocation.y,
            initialCameraLocation.z + currentPlayerLocation.z - initialPlayerLocation.z);
    }
}
