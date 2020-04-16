using EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventHandler = EventSystem.EventHandler;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private GameObject _player;
        private void Start()
        {
            DontDestroyOnLoad(this);
            SaveManager.Init();
            //Register Events
            EventHandler.RegisterListener<OnButtonStartEvent>(LoadLevel);
        }

        private void LoadLevel(OnButtonStartEvent obj)
        {
            SceneManager.LoadScene(obj.level);
        }

        private void OnDestroy()
        {
            //Unregister Events
            EventHandler.UnregisterListener<OnButtonStartEvent>(LoadLevel);
        }
    }
}
