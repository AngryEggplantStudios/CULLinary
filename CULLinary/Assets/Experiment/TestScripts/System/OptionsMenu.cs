using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgSlider;

    private void Start()
    {
        try
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFX_Vol");
            bgSlider.value = PlayerPrefs.GetFloat("BG_Vol");
        }
        catch
        {
            Debug.Log("No player prefs registered in options");
        }
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX_Vol", volume);
        PlayerPrefs.SetFloat("SFX_Vol", volume);
    }

    public void SetBGVolume(float volume)
    {
        audioMixer.SetFloat("BG_Vol", volume);
        PlayerPrefs.SetFloat("BG_Vol", volume);
    }
}
