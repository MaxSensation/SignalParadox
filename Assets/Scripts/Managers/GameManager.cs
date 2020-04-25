using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {

        private bool checkPointLoaded;
        private CheckPoint loadedCheckPoint;
        private void Awake()
        {
            SaveManager.Init();
            DontDestroyOnLoad(this);
            SaveManager.onLoadCheckPoint += LoadCheckPoint;
            SceneManager.sceneLoaded += LoadPlayerData;
        }

        private void LoadPlayerData(Scene arg0, LoadSceneMode arg1)
        {
            if (checkPointLoaded)
            {
                var player = GameObject.FindWithTag("Player");
                    
                player.transform.position = new Vector3(loadedCheckPoint.playerPosition[0], loadedCheckPoint.playerPosition[1], loadedCheckPoint.playerPosition[2]);
                var playerController = player.GetComponent<PlayerController.PlayerController>();
                playerController.hasStunBaton = loadedCheckPoint.hasStunBaton;
                playerController.hasStunGunUpgrade = loadedCheckPoint.hasStunGunUpgrade;
                checkPointLoaded = false;
            }
        }

        private void LoadCheckPoint(CheckPoint checkPoint)
        {
            loadedCheckPoint = checkPoint;
            checkPointLoaded = true;
            SceneManager.LoadScene(checkPoint.currentScene);
        }
    }
}
