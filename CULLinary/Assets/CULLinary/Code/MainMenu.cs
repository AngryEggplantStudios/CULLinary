using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public AudioSource HoverSound;

    [SerializeField] private GameObject Character;
    
    private Animator animator;

    void Start()
    {
        animator = Character.GetComponentInChildren<Animator>();
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        Debug.Log("Exit!");
        Application.Quit();
    }

    public void Hover()
    {
        HoverSound.Play();
    }
}
