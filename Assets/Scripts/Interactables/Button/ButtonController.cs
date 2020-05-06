//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using System.Collections;
using PlayerController;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables.Button
{
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] internal GameObject[] interactableObjects;
        [SerializeField] float  spamProtectionDelay;
        private StateMachine _stateMachine;
        private bool _isInRangeOfPlayer;
        private bool _isInteractable;
        private bool _spamprotectionOn;
        private WaitForSeconds _buttonDelay;
        private WaitForSeconds _spamProtectionDelay;

        public UnityEvent Activate;
        public static Action<GameObject[]> onButtonPressed;

        private void Awake()
        {
            _spamProtectionDelay = new WaitForSeconds(spamProtectionDelay);
            _isInteractable = true;
            _buttonDelay = new WaitForSeconds(2);
            PlayerInteractionTrigger.onInteractedEvent += OnButtonPressed;
        }

        private void OnButtonPressed(GameObject button)
        {
            if (button == gameObject && !_spamprotectionOn)
            {
                ButtonPress();
            }
        }

        private void OnDestroy()
        {
            PlayerInteractionTrigger.onInteractedEvent -= OnButtonPressed;
        }

        private IEnumerator ActivateButton()
        {
            yield return _buttonDelay;
            _isInteractable = true;
        }

        private IEnumerator SpamProtection()
        {
            _spamprotectionOn = true;
            yield return _spamProtectionDelay;
            _spamprotectionOn = false;
        }

        private void ButtonPress()
        {
            StartCoroutine(SpamProtection());
            _isInteractable = false;
            Activate?.Invoke();
            onButtonPressed?.Invoke(interactableObjects);
            StartCoroutine("ActivateButton");
        }
    }
}




