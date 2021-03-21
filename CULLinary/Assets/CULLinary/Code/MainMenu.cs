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
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject loadGameButtonNoData;
    [SerializeField] private GameObject loadGameButtonData;
    
    private Animator animator;

    private float fadeSpeed = 2f;
    private bool fading = false;
    private delegate void Action();
    private Action afterFade;
    private bool hasSavedData = false;

    void Start()
    {
        animator = character.GetComponentInChildren<Animator>();
        foreground.color = Color.clear;
        foreground.enabled = false;
        if (FileManager.CheckFile("saveFile.clown"))
        {
            loadGameButtonData.SetActive(true);
            loadGameButtonNoData.SetActive(false);
            hasSavedData = true;
        }
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
            PlayerData newPlayerData = new PlayerData();
            SaveSystem.SaveData(newPlayerData);
            playerManager.LoadData();
            //playerManager.InstantiateInventory();
            SceneManager.LoadScene(1); //Restaurant
             //Need to be changed to go to the loading screen in the future
        });
    }

    public void LoadGame()
    {
        Select();
        FadeToBlack(() => {
            playerManager.LoadData();
            //playerManager.InstantiateInventory();
            SceneManager.LoadScene(playerManager.GetCurrentIndex());
             //Need to be changed to go to the loading screen in the future
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

    public void LoadGameHover()
    {
        if (hasSavedData)
        {
            hoverSound.Play();
            animator.SetBool("hover", true);
        }
    }

    private void FadeToBlack(Action action)
    {
        fading = true;
        afterFade = action;
        foreground.enabled = true;
    }

    /**
    This method checks for saved data within the com and returns a bool
    */
    private bool isThereSavedData()
    {
        return false;
    }

}
