//Main author: Maximiliam Rosén

using Interactables.Triggers;
using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            TriggerMemo.onMemoPickupEvent += PlayMemo;
        }

        private void OnDestroy()
        {
            TriggerMemo.onMemoPickupEvent -= PlayMemo;
        }

        private void PlayMemo(AudioClip memo)
        {
            _audioSource.clip = memo;
            _audioSource.Play();
        }
    }
}
