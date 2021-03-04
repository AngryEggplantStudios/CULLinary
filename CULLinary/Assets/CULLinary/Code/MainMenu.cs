using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public AudioSource hoverSound;
    [SerializeField] public AudioSource selectSound;

    [SerializeField] private GameObject character;
    [SerializeField] private Image foreground;
    
    private Animator animator;

    private float fadeSpeed = 2f;
    private bool fading = false;
    private delegate void Action();
    private Action afterFade;

    void Start()
    {
        animator = character.GetComponentInChildren<Animator>();
        foreground.color = Color.clear;
        foreground.enabled = false;
    }

    void Update()
    {
        if (fading) {
            if (foreground.color.a <= 0.95f) {
                foreground.color = Color.Lerp(foreground.color, Color.black, fadeSpeed * Time.deltaTime);
            } else {
                fading = false;
                foreground.color = Color.black;
                afterFade();
            }
        }
    }

    public void NewGame()
    {
        Select();
        FadeToBlack(() => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }

    public void Options()
    {
        Select();
    }

    public void Exit()
    {
        Select();
        Debug.Log("Exit!");
        FadeToBlack(() => Application.Quit());
    }

    public void Hover()
    {
        hoverSound.Play();
        animator.SetBool("hover", true);
    }
    
    public void HoverOff()
    {
        animator.SetBool("hover", false);
    }

    private void Select()
    {
        selectSound.Play();
        animator.SetTrigger("select");
    }

    private void FadeToBlack(Action action)
    {
        fading = true;
        afterFade = action;
        foreground.enabled = true;
    }
}
