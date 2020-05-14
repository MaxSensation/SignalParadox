//Main author: Maximiliam Rosén

using System;
using PlayerController;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameObject _gameManager;
        private static bool _LoadingCheckPointNextSpawn, firstLoad;

        private void Awake()
        {
            if (_gameManager == null)
                _gameManager = gameObject;
            if (_gameManager != gameObject)
                Destroy(gameObject);
            SaveManager.Init();
            DontDestroyOnLoad(this);
            _LoadingCheckPointNextSpawn = false;
            firstLoad = true;
            PlayerController.PlayerController.onPlayerInit += LoadCheckPointOrPlayerData;
            PlayerController.PlayerController.onPlayerDeath += LoadCheckPointNextSpawn;
        }

        private static void LoadCheckPointOrPlayerData(GameObject player)
        {
            if (!firstLoad)
            {
                if (_LoadingCheckPointNextSpawn)
                {
                    SaveManager.LoadPlayerCheckPointData(player);
                }else
                    SaveManager.LoadPlayerData(player);
                _LoadingCheckPointNextSpawn = false;
            }
            else
            {
                player.GetComponent<HealthSystem>().ResetHealth();
            }
            firstLoad = false;
        }

        

        private static void LoadCheckPointNextSpawn()
        {
            _LoadingCheckPointNextSpawn = true;
        }
    }
}
