using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTimer : MonoBehaviour
{
    private static float gameTime;

    void Start()
    {
        gameTime = 0f;
    }

    private void Update()
    {
        gameTime += Time.deltaTime;
    }

    public static float GetTime()
    {
        return gameTime;
    }

    private void OnDestroy()
    {
        gameTime = 0f;
    }
}
