using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRaycaster : MonoBehaviour
{
    // Transform of the raycast layer. Only Y position is important
    public Transform raycastLayerTransform;
    // The Y position of the ground layer
    public float groundLayerY = 0;
    // Need the camera look at vector in order
    // to move the hit point towards the camera
    public Camera cam;

    // Performs raycasting with the correct layer
    // and modifies the hit result to align with the
    // given y-level of the ground layer
    public bool RaycastMouse(out RaycastHit hitInfo, float maxDistance = Mathf.Infinity)
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, maxDistance,
                            1 << LayerMask.NameToLayer("RaycastOnClick"))) {
        
            Vector3 lookAtVector = r.direction;
            Vector3 originalHitPoint = hit.point;

            // t * lookAtVector = vector difference of hit point and ground layer
            float t = (groundLayerY - originalHitPoint.y) / lookAtVector.y;
            Vector3 vectorDifference = t * lookAtVector;
            Vector3 groundPoint = originalHitPoint + vectorDifference;

            hitInfo = hit;
            hitInfo.distance = hit.distance + t;
            hitInfo.point = groundPoint;
            return true;
        } else {
            hitInfo = hit;
            return false;
        }
    }
}
