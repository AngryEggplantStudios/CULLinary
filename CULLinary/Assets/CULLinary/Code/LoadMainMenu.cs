using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Add functions that need to be called when main menu is loaded here
public class LoadMainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioClip victoryMusic;
    public AudioSource backgroundMusic;

    void Start()
    {
        OnMainMenuLoad();
    }

    private void OnMainMenuLoad()
    {
        // On main menu load, set Master_Vol to default
        audioMixer.SetFloat("Master_Vol", 0f);
        try
        {
            if (PlayerPrefs.GetInt("Just_Won_Game") == 1) {
                backgroundMusic.clip = victoryMusic;
                PlayerPrefs.SetInt("Just_Won_Game", 0);
                backgroundMusic.Play();
            }
        }
        catch (Exception e)
        {
            Debug.Log("No player prefs registered in options");
            Debug.Log(e);
        }
    }
}
