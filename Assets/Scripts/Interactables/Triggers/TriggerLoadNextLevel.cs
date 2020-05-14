//Main author: Maximiliam Rosén

using System;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactables.Triggers
{
    public class TriggerLoadNextLevel : MonoBehaviour
    {
        public static Action<PlayerData> onWantToLoadNextLevelEvent;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var playerData = new PlayerData(other.gameObject, false);
                onWantToLoadNextLevelEvent?.Invoke(playerData);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
