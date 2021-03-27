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
        SceneManager.LoadScene(2);
        gameObject.SetActive(false);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
        gameObject.SetActive(false);
    }
}
