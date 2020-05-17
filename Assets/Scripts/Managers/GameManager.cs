//Main author: Maximiliam Rosén

using SaveSystem;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameObject _gameManager;

        private void Awake()
        {
            if (_gameManager == null)
                _gameManager = gameObject;
            if (_gameManager != gameObject)
                Destroy(gameObject);
            else
            {
                SaveManager.Init();
                DontDestroyOnLoad(this);
            }
        }
    }
}
