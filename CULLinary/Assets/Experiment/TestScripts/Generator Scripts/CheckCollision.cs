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

    public void SetIsNotCollided()
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

    public void TurnOnCollider()
    {
        GetComponent<BoxCollider>().enabled = true;
    }


    public void TurnOffCollider()
    {
        GetComponent<BoxCollider>().enabled = false;
    }
}
