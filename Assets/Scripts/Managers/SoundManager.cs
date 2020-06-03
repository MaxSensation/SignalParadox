//Main author: Maximiliam Rosén

using Interactables.Triggers.Memo;
using Player;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        private AudioSource audioSource;

        private void Awake()
        {
            PlayerController.onPlayerInitEvent += player => audioMixer.FindSnapshot("Default").TransitionTo(1f);
            audioSource = GetComponent<AudioSource>();
            TriggerMemo.onMemoPickupEvent += PlayMemo;
        }

        private void OnDestroy()
        {
            PlayerController.onPlayerInitEvent -= player => audioMixer.FindSnapshot("Default").TransitionTo(1f);
            TriggerMemo.onMemoPickupEvent -= PlayMemo;
        }

        private void PlayMemo(AudioClip memo)
        {
            audioSource.clip = memo;
            audioSource.Play();
        }
    }
}
