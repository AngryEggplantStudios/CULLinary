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
        if (currScene.buildIndex == (int)SceneIndexes.TUT_DUNGEON)
        {
            SceneManager.LoadScene((int)SceneIndexes.TUT_RETURN);
        }
        else if (currScene.buildIndex == (int)SceneIndexes.TUT_BOSS)
        {
            SceneManager.LoadScene((int)SceneIndexes.DUNGEON);
        }
        else
        {
            SceneManager.LoadScene((int)SceneIndexes.DUNGEON);
        }
        gameObject.SetActive(false);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene((int)SceneIndexes.MAINMENU);
        gameObject.SetActive(false);
    }
}
