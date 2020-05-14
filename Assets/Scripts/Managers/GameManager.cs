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
        private static bool _loadingCheckPointNextSpawn, _firstLoad;

        private void Awake()
        {
            if (_gameManager == null)
                _gameManager = gameObject;
            if (_gameManager != gameObject)
                Destroy(gameObject);
            SaveManager.Init();
            DontDestroyOnLoad(this);
            _loadingCheckPointNextSpawn = false;
            _firstLoad = true;
            PlayerController.PlayerController.onPlayerInit += LoadCheckPointOrPlayerData;
            PlayerController.PlayerController.onPlayerDeath += LoadCheckPointNextSpawn;
        }

        private static void LoadCheckPointOrPlayerData(GameObject player)
        {
            if (!_firstLoad)
            {
                if (_loadingCheckPointNextSpawn)
                {
                    SaveManager.LoadPlayerCheckPointData(player);
                }else
                    SaveManager.LoadPlayerData(player);
                _loadingCheckPointNextSpawn = false;
            }
            else
            {
                player.GetComponent<HealthSystem>().ResetHealth();
            }
            _firstLoad = false;
        }

        

        private static void LoadCheckPointNextSpawn()
        {
            _loadingCheckPointNextSpawn = true;
        }
    }
}
