//Main author: Maximiliam Rosén

using SaveSystem;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameObject gameManager;

        private void Awake()
        {
            if (gameManager == null)
                gameManager = gameObject;
            if (gameManager != gameObject)
                Destroy(gameObject);
            else
            {
                SaveManager.Init();
                DontDestroyOnLoad(this);
            }
        }
    }
}