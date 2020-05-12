//Main author: Maximiliam Rosén

using System;
using PlayerController;
using SaveSystem;
using SaveSystem.CheckPointSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameObject _gameManager;
        private static CheckPoint _currentCheckPoint;
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
        }

        private void GivePlayerMaxHP(Scene arg0, LoadSceneMode arg1)
        {
                GameObject.Find("Player").GetComponent<HealthSystem>().ResetHealth();
                SceneManager.sceneLoaded -= GivePlayerMaxHP;
        }
    }
}
