//Main author: Maximiliam Rosén

using Interactables.Triggers.Memo;
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
            TriggerMemo.onMemoPickupEvent -= PlayMemo;
        }

        private void PlayMemo(AudioClip memo)
        {
            audioSource.clip = memo;
            audioSource.Play();
        }
    }
}
