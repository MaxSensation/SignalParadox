//Main author: Maximiliam Rosén

using System;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactables.Triggers.Events
{
    public class TriggerLoadNextLevel : MonoBehaviour
    {
        public static Action<PlayerData> onTriggerdNextLevelEvent;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var playerData = new PlayerData(other.gameObject);
                onTriggerdNextLevelEvent?.Invoke(playerData);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
