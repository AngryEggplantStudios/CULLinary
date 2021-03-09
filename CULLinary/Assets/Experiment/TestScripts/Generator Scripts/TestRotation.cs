using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    [SerializeField] private GameObject otherConnectionPoint;
    [SerializeField] private GameObject parentRef;
    [SerializeField] private float bias;
    void Start()
    {
        Transform parentTransform = parentRef.transform;
        Transform otherTransform = otherConnectionPoint.transform;
        Vector3 otherForwardVector = otherTransform.forward;
        parentTransform.rotation = Quaternion.LookRotation(otherForwardVector);
        float currentEulerAngle = parentTransform.eulerAngles.y;
        float xBias = 0f;
        float zBias = 0f;
        if (Mathf.Approximately(currentEulerAngle, 0.0f))
        {
            zBias = bias;   
        }
        else if (Mathf.Approximately(currentEulerAngle, 90.0f))
        {
            xBias = bias;
        }
        else if (Mathf.Approximately(currentEulerAngle, 180f))
        {
            zBias = -bias;
        }
        else if (Mathf.Approximately(currentEulerAngle, 270.0f))
        {
            xBias = -bias;
        }
        parentTransform.position = new Vector3(otherTransform.position.x + xBias, otherTransform.position.y, otherTransform.position.z + zBias);
    }
}
