//Main author: Maximiliam Rosén

using System;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactables.Triggers
{
    public class TriggerLoadNextLevel : MonoBehaviour
    {
        public static Action<PlayerData> onLoadedNextLevelEvent;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                var playerData = new PlayerData(false);
                onLoadedNextLevelEvent?.Invoke(playerData);
            }
        }
    }
}
