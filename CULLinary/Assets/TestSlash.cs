using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSlash : MonoBehaviour
{

    [SerializeField] private GameObject weapon;
    public void ToggleColliderOn()
    {
        weapon.GetComponent<Collider>().enabled = true;
    }

    public void ToggleColliderOff()
    {
        weapon.GetComponent<Collider>().enabled = false;
    }
}
