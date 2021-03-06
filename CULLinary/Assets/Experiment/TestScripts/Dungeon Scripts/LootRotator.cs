using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootRotator : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(new Vector3(10, 45, 10) * Time.deltaTime);
    }
}
