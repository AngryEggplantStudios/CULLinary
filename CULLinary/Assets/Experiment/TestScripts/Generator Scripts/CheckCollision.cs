using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    [SerializeField] bool isEnd;
    [SerializeField] private bool isCollided = false;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Validator"))
        {
            isCollided = true;
        }
        
    }

    public bool GetIsCollided()
    {
        return isCollided;
    }

    public bool GetIsEnd()
    {
        return isEnd;
    }

    private void OnTriggerExit(Collider collider)
    {
        isCollided = false;
    }

    private void Awake()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    public void TurnOnCollider()
    {
        GetComponent<BoxCollider>().enabled = true;
    }
}
