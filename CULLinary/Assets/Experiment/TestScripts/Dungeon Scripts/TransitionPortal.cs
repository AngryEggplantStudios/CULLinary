using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPortal : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    [SerializeField] private GameObject playerManagerObj;
    
    private bool isTriggered;
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = playerManagerObj.GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isTriggered)
        {

            Debug.Log("Player can transit");
            playerManager.SaveData();
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
