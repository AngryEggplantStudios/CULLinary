using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPortal : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    
    private bool isTriggered;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isTriggered)
        {
            Debug.Log("Player can transit");
            LoadScene();
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            isTriggered = false;
        }
    }
}
