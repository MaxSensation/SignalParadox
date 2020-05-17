using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadMainMenu : MonoBehaviour
{
    private VideoPlayer _videoPlayer;

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.loopPointReached += LoadMainMenuScene;
    }

    private void LoadMainMenuScene(VideoPlayer source)
    {
        SceneManager.LoadScene("GameMenu");
    }
}
