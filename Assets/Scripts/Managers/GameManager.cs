using EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventHandler = EventSystem.EventHandler;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private GameObject _player;
        private int totalCollectedMemos;
        private void Start()
        {
            DontDestroyOnLoad(this);
            SceneManager.LoadScene("ShowCase");
            SaveManager.Init();
            //Register Events
            EventHandler.RegisterListener<OnTriggerMemoEvent>(MemoCollected);
        }

        private void OnDestroy()
        {
            //Unregister Events
            EventHandler.UnregisterListener<OnTriggerMemoEvent>(MemoCollected);
        }

        private void MemoCollected(OnTriggerMemoEvent obj)
        {
            totalCollectedMemos++;
        }
    }
}
