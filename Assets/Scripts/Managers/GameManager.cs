﻿//Main author: Maximiliam Rosén

using Interactables.CheckPointSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameObject gameManager;
        private bool checkPointLoaded;
        private CheckPoint loadedCheckPoint;
        private void Awake()
        {
            if (gameManager == null)
                gameManager = gameObject;
            if (gameManager != gameObject)
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
        }
    }
}
