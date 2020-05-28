//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using System.Collections;
using Interactables.Triggers.EntitiesTrigger;
using UnityEngine;

namespace Interactables.Button
{
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] private ButtonStates currentState;
        [SerializeField] private GameObject[] interactableObjects;
        [SerializeField] private float  spamProtectionDelay;
        [Tooltip("SoundEffects")][SerializeField] private AudioClip accessGrantedSound, accessDeniedSound;
        private bool spamprotectionOn;
        private AudioSource audioSource;
        private WaitForSeconds spamProtectionDelaySeconds;

        public static Action<GameObject[]> onButtonPressedEvent;
        public Action<ButtonStates> onStateChangeEvent;
        public enum ButtonStates { Standby, Activated, Locked }
        
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            spamProtectionDelaySeconds = new WaitForSeconds(spamProtectionDelay);
            InteractionTrigger.onInteractedEvent += OnButtonPressed;
        }

        private void Start() => onStateChangeEvent?.Invoke(currentState);

        private void OnDestroy() => InteractionTrigger.onInteractedEvent -= OnButtonPressed;
        
        private void OnButtonPressed(GameObject button)
        {
            if (button != gameObject || spamprotectionOn) return;
            if (currentState != ButtonStates.Locked)
                ButtonPress();
            else
                audioSource.PlayOneShot(accessDeniedSound);
            StartCoroutine(SpamProtection());
        }
        
        private IEnumerator SpamProtection()
        {
            spamprotectionOn = true;
            yield return spamProtectionDelaySeconds;
            if (currentState != ButtonStates.Locked)
            {
                currentState = ButtonStates.Standby;
                onStateChangeEvent?.Invoke(currentState);   
            }
            spamprotectionOn = false;
        }
        
        [ContextMenu("PressButton")]
        public void ButtonPress()
        {
            audioSource.PlayOneShot(accessGrantedSound);
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