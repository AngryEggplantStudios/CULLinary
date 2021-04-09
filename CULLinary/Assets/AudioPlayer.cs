using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    [SerializeField] private float timeToStart;
    void Start()
    {
        StartCoroutine(StartAudio());
    }

    private IEnumerator StartAudio()
    {
        yield return new WaitForSeconds(timeToStart);
        audio.Play();
    }
}
