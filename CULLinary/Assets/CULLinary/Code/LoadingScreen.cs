using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Add functions that need to be called when a scene is loaded here
public class LoadingScreen : MonoBehaviour
{
    public AudioMixer audioMixer;

    void Start()
    {
        OnSceneLoad();
    }

    private void OnSceneLoad()
    {
        // On scene load, set Master_Vol to default
        audioMixer.SetFloat("Master_Vol", 0f);
    }
}
