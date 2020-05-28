//Main author: Ferreira Dos Santos Keziah
//Secondary author: Andreas Berzelius

using UnityEngine;
using Player.PlayerStateMachine;
using System.Collections.Generic;

namespace FootStepSound
{
    public class DynamicFootStep : MonoBehaviour
    {
        [SerializeField] private AudioClip[] surfaceSounds;
        [SerializeField] private float defaultVolume = 0.2f;
        [SerializeField] private float crouchVolume = 0.1f;
        private Dictionary<SurfaceColliderType.SurfaceTypes, AudioClip> soundDictionary;
        private AudioSource audiosource;
        private double time;
        private float filterTime;
        private SurfaceColliderType.SurfaceTypes currentSurfaceType;

        private void Awake()
        {
            soundDictionary = new Dictionary<SurfaceColliderType.SurfaceTypes, AudioClip>();
            soundDictionary.Add(SurfaceColliderType.SurfaceTypes.Metal, surfaceSounds[0]);
            soundDictionary.Add(SurfaceColliderType.SurfaceTypes.Stairs, surfaceSounds[1]);
            soundDictionary.Add(SurfaceColliderType.SurfaceTypes.Glass, surfaceSounds[2]);
            currentSurfaceType = SurfaceColliderType.SurfaceTypes.Metal;
            audiosource = GetComponent<AudioSource>();
            audiosource.volume = 0.2f;
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

        private void OnEnteredCrouch() => audiosource.volume = crouchVolume;

        private void OnExitedCrouch() => audiosource.volume = defaultVolume;


        private void PlayFootstepSound()
        {
            if (AudioSettings.dspTime < time + filterTime) return;
            time = AudioSettings.dspTime;
            audiosource.PlayOneShot(soundDictionary[currentSurfaceType]);
        }
    }
}
