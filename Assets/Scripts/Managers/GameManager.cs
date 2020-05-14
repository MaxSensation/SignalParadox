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
        private static CheckPoint _currentCheckPoint;
        private static GameObject _player;
        private void Awake()
        {
            if (_gameManager == null)
                _gameManager = gameObject;
            if (_gameManager != gameObject)
                Destroy(gameObject);
            SaveManager.Init();
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            SceneManager.sceneLoaded += GivePlayerMaxHP;
            SceneManager.sceneLoaded += LoadPlayerData;
        }

        private void LoadPlayerData(Scene arg0, LoadSceneMode arg1)
        {
            if (SaveManager.HasPlayerData())
            {
                UpdatePlayer(GameObject.Find("Player"));
                SaveManager.LoadPlayerData();
            }
        }

        private void GivePlayerMaxHP(Scene arg0, LoadSceneMode arg1)
        {
                GameObject.Find("Player").GetComponent<HealthSystem>().ResetHealth();
                SceneManager.sceneLoaded -= GivePlayerMaxHP;
        }

        public static void UpdatePlayer(GameObject player)
        {
            _player = player;
        }

        public static GameObject GetPlayer()
        {
            return _player;
        }
    }
}
