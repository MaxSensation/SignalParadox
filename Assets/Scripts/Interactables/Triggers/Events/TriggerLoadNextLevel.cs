//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using Player;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

namespace Interactables.Triggers.Events
{
    public class TriggerLoadNextLevel : MonoBehaviour
    {
        [SerializeField] [Tooltip("the black screen fading in between levels")] private Animator loadingScreenAnimator;
        [SerializeField] private AudioMixer audioMixer;
        private AudioSource audioSource;
        private PlayerController player;
        public static Action<PlayerData> onTriggerdNextLevelEvent;
        private bool isActivated;

        private void Awake()
        {
            player = FindObjectOfType<PlayerController>();
            audioSource = GetComponent<AudioSource>();
        }


        private void Update()
        {
            if (isActivated && !audioSource.isPlaying)
                LoadNextLevel();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var playerData = new PlayerData(other.gameObject);
            onTriggerdNextLevelEvent?.Invoke(playerData);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            player.Velocity = Vector3.zero;
            player.InCinematic = true;
            loadingScreenAnimator.SetBool("ReachedEnd", true);
            audioMixer.FindSnapshot("LevelTransition").TransitionTo(0.1f);
            audioSource.Play();
            isActivated = true;
        }

        private static void LoadNextLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
