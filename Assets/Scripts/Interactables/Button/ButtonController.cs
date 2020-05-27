﻿//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using System.Collections;
using Interactables.Triggers;
using Interactables.Triggers.EntitiesTrigger;
using Player;
using UnityEngine;

namespace Interactables.Button
{
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] private ButtonStates currentState;
        [SerializeField] private GameObject[] interactableObjects;
        [SerializeField] private float  spamProtectionDelay;
        [Tooltip("SoundEffects")][SerializeField] private AudioClip accessGrantedSound, accessDeniedSound;
        private bool _isInRangeOfPlayer, _spamprotectionOn;
        private AudioSource _audioSource;
        private WaitForSeconds _spamProtectionDelay;

        public static Action<GameObject[]> onButtonPressedEvent;
        public Action<ButtonStates> onStateChangeEvent;
        public enum ButtonStates { Standby, Activated, Locked }
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _spamProtectionDelay = new WaitForSeconds(spamProtectionDelay);
            InteractionTrigger.onInteractedEvent += OnButtonPressed;
        }

        private void Start()
        {
            onStateChangeEvent?.Invoke(currentState);
        }

        private void OnDestroy()
        {
            InteractionTrigger.onInteractedEvent -= OnButtonPressed;
        }
        
        private void OnButtonPressed(GameObject button)
        {
            if (button != gameObject || _spamprotectionOn) return;
            if (currentState != ButtonStates.Locked)
                ButtonPress();
            else
                _audioSource.PlayOneShot(accessDeniedSound);
            StartCoroutine(SpamProtection());
        }
        
        private IEnumerator SpamProtection()
        {
            _spamprotectionOn = true;
            yield return _spamProtectionDelay;
            if (currentState != ButtonStates.Locked)
            {
                currentState = ButtonStates.Standby;
                onStateChangeEvent?.Invoke(currentState);   
            }
            _spamprotectionOn = false;
        }
        
        [ContextMenu("PressButton")]
        public void ButtonPress()
        {
            _audioSource.PlayOneShot(accessGrantedSound);
            currentState = ButtonStates.Activated;
            onStateChangeEvent?.Invoke(currentState);
            onButtonPressedEvent?.Invoke(interactableObjects);
        }
        
        [ContextMenu("Lock")]
        public void Lock()
        {
            currentState = ButtonStates.Locked;
            onStateChangeEvent?.Invoke(currentState);
        }
    }
}




