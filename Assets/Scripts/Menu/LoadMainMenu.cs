//Main author: Maximiliam Rosén

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadMainMenu : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += LoadMainMenuScene;
    }

    private void LoadMainMenuScene(VideoPlayer source)
    {
        SceneManager.LoadScene("GameMenu");
    }
}
