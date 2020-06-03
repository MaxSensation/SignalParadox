//Main author: Maximiliam Rosén

using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Menu
{
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
            SaveManager.SaveGame();
            SceneManager.LoadScene("GameMenu");
        }
    }
}
