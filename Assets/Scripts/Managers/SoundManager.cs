using System;
using EventSystem;
using UnityEngine;
using EventHandler = EventSystem.EventHandler;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            EventHandler.RegisterListener<OnTriggerMemoEvent>(PlayMemo);
        }

        private void OnDestroy()
        {
            EventHandler.UnregisterListener<OnTriggerMemoEvent>(PlayMemo);
        }

        private void PlayMemo(OnTriggerMemoEvent obj)
        {
            _audioSource.clip = obj.memoAudioClip;
            _audioSource.Play();
        }
    }
}
