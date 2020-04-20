using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            SaveManager.Init();
            DontDestroyOnLoad(this);
            SaveManager.onLoadCheckPoint += LoadPlayerData;
        }

        private void LoadPlayerData(CheckPoint checkPoint)
        {
            SceneManager.LoadScene(checkPoint.currentScene);
            var player = GameObject.FindWithTag("Player");
            player.transform.position = new Vector3(checkPoint.playerPosition[0], checkPoint.playerPosition[1], checkPoint.playerPosition[2]);
        }
    }
}
