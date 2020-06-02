//Main author: Maximiliam Rosén

using System;
using Player;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace Interactables.Triggers.Events
{
    public class TriggerLoadNextLevel : MonoBehaviour
    {
        [SerializeField] [Tooltip("the black screen fading in between levels")] private GameObject loadingScreenObject;
        [SerializeField] private AudioMixer audioMixer;
        private AudioSource audioSource;
        private PlayerController player;
        private Animator loadingScreenAnimator;
        private Image loadingScreenImage;
        public static Action<PlayerData> onTriggerdNextLevelEvent;

        private void Awake()
        {
            player = FindObjectOfType<PlayerController>();
            loadingScreenAnimator = loadingScreenObject.GetComponent<Animator>();
            loadingScreenImage = loadingScreenObject.GetComponent<Image>();
            audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var playerData = new PlayerData(other.gameObject);
                onTriggerdNextLevelEvent?.Invoke(playerData);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                player.Velocity = Vector3.zero;
                player.InCinematic = true;
                loadingScreenAnimator.SetBool("ReachedEnd", true);
                audioMixer.FindSnapshot("LevelTransition").TransitionTo(0.1f);
                audioSource.Play();
                if (loadingScreenImage.color.a == 1)
                    LoadNextLevel();
            }
        }

        public void LoadNextLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
