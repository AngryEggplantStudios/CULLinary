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
        //Debug.Log("I have triggered!");
        if (collider.CompareTag("Validator"))
        {
            //Debug.Log("Why now then you fire");
            isCollided = true;
            //Debug.Log("I have set the boolean flag true!");
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

    private void OnTriggerExit(Collider collider)
    {
        //Debug.Log("This happened");
        //isCollided = false;
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
