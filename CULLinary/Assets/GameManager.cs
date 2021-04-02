using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void GameOver()
    {
        gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        Scene currScene = SceneManager.GetActiveScene();
        if (currScene.name == "Tut_dungeon")
        {
            SceneManager.LoadScene(4);
        }
        else // Reload normal dungeon scene
        {
            SceneManager.LoadScene(2);
        }

        gameObject.SetActive(false);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
        gameObject.SetActive(false);
    }
}
