//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius
//Third author: Ferreira Dos Santos Keziah

using UnityEngine;
using Player.PlayerStateMachine;
using System.Collections.Generic;

namespace FootStepSound
{
    public class DynamicFootStep : MonoBehaviour
    {
        [SerializeField] private AudioSource footstepAudioSource;
        [SerializeField] private AudioClip[] metalSounds;
        [SerializeField] private AudioClip[] stairsSounds;
        [SerializeField] private AudioClip[] glassSounds;
        [SerializeField] private float defaultVolume = 0.2f;
        [SerializeField] private float crouchVolume = 0.1f;
        private Dictionary<SurfaceColliderType.SurfaceTypes, AudioClip[]> soundDictionary;
        private double time;
        private float filterTime;
        private SurfaceColliderType.SurfaceTypes currentSurfaceType;

        private void Awake()
        {
            soundDictionary = new Dictionary<SurfaceColliderType.SurfaceTypes, AudioClip[]>();
            soundDictionary.Add(SurfaceColliderType.SurfaceTypes.Metal, metalSounds);
            soundDictionary.Add(SurfaceColliderType.SurfaceTypes.Stairs, stairsSounds);
            soundDictionary.Add(SurfaceColliderType.SurfaceTypes.Glass, glassSounds);
            currentSurfaceType = SurfaceColliderType.SurfaceTypes.Metal;
            footstepAudioSource.volume = defaultVolume;
            time = AudioSettings.dspTime;
            filterTime = 0.2f;
            CrouchState.onEnteredCrouchEvent += OnEnteredCrouch;
            CrouchState.onExitCrouchEvent += OnExitedCrouch;
            SurfaceColliderType.onEnteredSurfaceZoneEvent += surfaceType => currentSurfaceType = surfaceType;
        }

        private void OnDestroy()
        {
            CrouchState.onEnteredCrouchEvent -= OnEnteredCrouch;
            CrouchState.onExitCrouchEvent -= OnExitedCrouch;
            SurfaceColliderType.onEnteredSurfaceZoneEvent -= surfaceType => currentSurfaceType = surfaceType;
        }

        private void OnEnteredCrouch() => footstepAudioSource.volume = crouchVolume;

        private void OnExitedCrouch() => footstepAudioSource.volume = defaultVolume;


        public void PlayFootstepSound()
        {
            if (AudioSettings.dspTime < time + filterTime) return;
            time = AudioSettings.dspTime;
            var soundList = soundDictionary[currentSurfaceType];
            if (soundList.Length > 0)
                footstepAudioSource.PlayOneShot(soundList[Random.Range(0, soundList.Length)]);
        }
    }
}
