using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public AudioSource hoverSound;
    [SerializeField] public AudioSource selectSound;

    [SerializeField] private GameObject character;
    [SerializeField] private Image foreground;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject mainButtonsMenu;
    [SerializeField] private GameObject loadGameButton;
    [SerializeField] private GameObject warningNewGame;

    public AudioMixer audioMixer;
    
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
        if (!FileManager.CheckFile("saveFile.clown"))
        {
            Button loadButton = loadGameButton.GetComponent<Button>();
            loadButton.interactable = false;
            ColorBlock cb = loadButton.colors;
            cb.normalColor = Color.gray;
            loadButton.colors = cb;
        }
        else
        {
            hasSavedData = true;
        }
    }

    void Update()
    {
        float targetVolume = 0.0001f;
        string channel = "Master_Vol";
        if (fading) {
            if (foreground.color.a <= 0.95f) {

                float rawVolume;
                audioMixer.GetFloat(channel, out rawVolume);
                float currentVolume = Mathf.Pow(10, rawVolume / 20);
                float newVolume = Mathf.Lerp(currentVolume, targetVolume, fadeSpeed * Time.deltaTime);
                audioMixer.SetFloat("Master_Vol", Mathf.Log10(newVolume) * 20);

                foreground.color = Color.Lerp(foreground.color, Color.black, fadeSpeed * Time.deltaTime);
            } else {
                fading = false;
                foreground.color = Color.black;
                // Options Menu will handle setting Master_Vol
                // back to 0 on new scene load
                afterFade();
            }
        }
    }

    public void NewGame()
    {
        Select();
        if (hasSavedData)
        {
            warningNewGame.SetActive(true);
        }
        else
        {
            PlayNewGame();
        }
    }

    public void CancelNewGame()
    {
        warningNewGame.SetActive(false);
    }

    public void PlayNewGame()
    {
        FadeToBlack(() => {
            PlayerData newPlayerData = new PlayerData();
            SaveSystem.SaveData(newPlayerData);
            PlayerManager.LoadData();
            SceneManager.LoadScene((int) SceneIndexes.INTRO); //Restaurant -1
            //Need to be changed to go to the loading screen in the future
        });
    }

    public void LoadGame()
    {
        Select();
        FadeToBlack(() => {
            PlayerManager.LoadData();
            SceneManager.LoadScene(PlayerManager.playerData.GetCurrentIndex());
             //Need to be changed to go to the loading screen in the future
        });
    }

    public void Options()
    {
        Select();
        optionsMenu.SetActive(true);
        mainButtonsMenu.SetActive(false);
    }

    public void Back()
    {
        Select();
        mainButtonsMenu.SetActive(true);
        optionsMenu.SetActive(false);
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
