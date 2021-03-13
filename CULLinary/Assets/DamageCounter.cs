using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCounter : MonoBehaviour
{
    public float destroyTime = 1f;
    public Vector3 offset = new Vector3(0, 50, 0);
    void Start()
    {
        Destroy(gameObject, destroyTime);
        transform.localPosition += offset;
    }
}
