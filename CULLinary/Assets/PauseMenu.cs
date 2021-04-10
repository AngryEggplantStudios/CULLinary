using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    [SerializeField] private GameObject pauseMenuUI;

    [SerializeField] private bool isThereLoader;
    [SerializeField] private SceneIndexes levelToRestart;

    [SerializeField] private GameObject mainPauseButtons;
    [SerializeField] private GameObject optionButtons;

    [SerializeField] private GameObject warningScreen;
    [SerializeField] private GameObject warningRestart;
    [SerializeField] private GameObject controlsScreen;
 
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (DungeonLoader.isDoneLoading || !isThereLoader))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        mainPauseButtons.SetActive(true);
        optionButtons.SetActive(false);
        pauseMenuUI.SetActive(false);
        warningRestart.SetActive(false);
        warningScreen.SetActive(false);
        controlsScreen.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;

        Scene currScene = SceneManager.GetActiveScene();
        if (currScene.name == "TestRestaurant")
        {
            CookingStation cookingStation = GameObject.Find("Recipe Controller").GetComponent<CookingStation>();
            if (cookingStation.isCooking == true)
                cookingStation.DisableMovementOfPlayer();
        }
    }

    public void Options()
    {
        optionButtons.SetActive(true);
        mainPauseButtons.SetActive(false);
    }

    public void Back()
    {
        optionButtons.SetActive(false);
        controlsScreen.SetActive(false);
        warningScreen.SetActive(false);
        warningRestart.SetActive(false);
        mainPauseButtons.SetActive(true);
    }

    public void Controls()
    {
        controlsScreen.SetActive(true);
        mainPauseButtons.SetActive(false);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene((int)levelToRestart);
        pauseMenuUI.SetActive(false);
    }

    public void WarningBeforeRestart()
    {
        warningRestart.SetActive(true);
        mainPauseButtons.SetActive(false);
    }

    public void WarningBeforeMainMenu()
    {
        warningScreen.SetActive(true);
        mainPauseButtons.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene((int)SceneIndexes.MAINMENU);
        pauseMenuUI.SetActive(false);
    }

}
