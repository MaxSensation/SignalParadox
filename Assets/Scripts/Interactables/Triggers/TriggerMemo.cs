//Main author: Maximiliam Rosén

using System;
using UnityEngine;

namespace Interactables.Triggers
{
    public class TriggerMemo : MonoBehaviour
    {
        [SerializeField] private AudioClip memo;
        private bool hasPlayed;
        private AudioSource _audioSource;
        public static Action<AudioClip> onMemoPickup;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!hasPlayed && other.CompareTag("Player"))
            {
                Debug.Log("Memo Found");
                hasPlayed = true;
                onMemoPickup?.Invoke(memo);
                _audioSource.Stop();
            }
        }
    }
}
