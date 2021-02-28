using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocateCamera : MonoBehaviour
{
    public Transform currentPlayerTransform;
    public float scale = 1.0f;
    public float minCameraDistance = 1.0f;

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
        if (Input.mouseScrollDelta.y != 0) {
            Vector3 playerToCamera = initialCameraLocation - initialPlayerLocation;
            float distance = playerToCamera.magnitude;
            float zoomChange = -Input.mouseScrollDelta.y * scale;
            if (distance + zoomChange > minCameraDistance) {
                playerToCamera = playerToCamera * (distance + zoomChange) / distance;
                initialCameraLocation = initialPlayerLocation + playerToCamera;
            }
        }

        transform.position = new Vector3(
            initialCameraLocation.x + currentPlayerLocation.x - initialPlayerLocation.x,
            initialCameraLocation.y,
            initialCameraLocation.z + currentPlayerLocation.z - initialPlayerLocation.z);
    }
}
