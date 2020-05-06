//Main author: Maximiliam Rosén

using Interactables.CheckPointSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameObject _gameManager;
        private bool checkPointLoaded;
        private CheckPoint loadedCheckPoint;
        private void Awake()
        {
            if (_gameManager == null)
                _gameManager = gameObject;
            if (_gameManager != gameObject)
                Destroy(gameObject);
            SaveManager.Init();
            DontDestroyOnLoad(this);
            SaveManager.onLoadCheckPoint += LoadCheckPoint;
            SceneManager.sceneLoaded += LoadPlayerData;
        }

        private void LoadPlayerData(Scene arg0, LoadSceneMode arg1)
        {
            if (!checkPointLoaded) return;
            var player = GameObject.FindWithTag("Player");
            player.transform.position = new Vector3(loadedCheckPoint.playerPosition[0], loadedCheckPoint.playerPosition[1], loadedCheckPoint.playerPosition[2]);
            checkPointLoaded = false;
        }

        private void LoadCheckPoint(CheckPoint checkPoint)
        {
            loadedCheckPoint = checkPoint;
            checkPointLoaded = true;
            SceneManager.LoadScene(checkPoint.currentScene);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
