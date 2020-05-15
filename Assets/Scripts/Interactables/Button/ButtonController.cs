//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using System.Collections;
using PlayerController;
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

        public static Action<GameObject[]> onButtonPressed;
        public Action<ButtonStates> onStateChangeEvent;
        public enum ButtonStates { Standby, Activated, Locked }
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _spamProtectionDelay = new WaitForSeconds(spamProtectionDelay);
            PlayerInteractionTrigger.onInteractedEvent += OnButtonPressed;
        }

        private void Start()
        {
            onStateChangeEvent?.Invoke(currentState);
        }

        private void OnDestroy()
        {
            PlayerInteractionTrigger.onInteractedEvent -= OnButtonPressed;
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
            currentState = ButtonStates.Standby;
            onStateChangeEvent?.Invoke(currentState);
            _spamprotectionOn = false;
        }
        
        [ContextMenu("PressButton")]
        private void ButtonPress()
        {
            _audioSource.PlayOneShot(accessGrantedSound);
            currentState = ButtonStates.Activated;
            onStateChangeEvent?.Invoke(currentState);
            onButtonPressed?.Invoke(interactableObjects);
        }
        
        public void Lock()
        {
            currentState = ButtonStates.Locked;
            onStateChangeEvent?.Invoke(currentState);
        }
    }
}




