using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootRotator : MonoBehaviour
{
    public float amplitude = 0.05f;
    public float verticalSpeed = 2f;
    public float rotationSpeed = 90;

    private void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);

        Vector3 pos = transform.position;
        float newY = (Mathf.Sin(Time.time * verticalSpeed) + 1) * amplitude;
        transform.position = new Vector3(pos.x, newY, pos.z);
    }
}
