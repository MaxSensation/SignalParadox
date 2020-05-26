﻿//Main author: Maximiliam Rosén

using Interactables.Triggers;
using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            TriggerMemo.onMemoPickupEvent += PlayMemo;
        }

        private void OnDestroy()
        {
            TriggerMemo.onMemoPickup -= PlayMemo;
        }

        private void PlayMemo(AudioClip memo)
        {
            audioSource.clip = memo;
            audioSource.Play();
        }
    }
}
