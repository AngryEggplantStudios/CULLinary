using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    [SerializeField] bool isEnd;
    [SerializeField] private bool isCollided = false;
    [SerializeField] private bool isStart;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Validator"))
        {
            isCollided = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        isCollided = false;
    }

    public bool GetIsCollided()
    {
        return isCollided;
    }

    public bool GetIsEnd()
    {
        return isEnd;
    }

    private void Start()
    {
        if (!isStart)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
        
    }

    public void TurnOnCollider()
    {
        GetComponent<BoxCollider>().enabled = true;
    }
}
