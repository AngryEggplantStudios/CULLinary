using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    private VideoPlayer video;
    [SerializeField] private SceneIndexes sceneIndex;
    private void Awake()
    {
        video = GetComponent<VideoPlayer>();
        video.loopPointReached += CheckOver;
    }

    private void CheckOver(VideoPlayer videoPlayer)
    {
        SceneManager.LoadScene((int)sceneIndex);
    }
}
