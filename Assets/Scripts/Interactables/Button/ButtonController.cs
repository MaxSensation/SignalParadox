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
        [SerializeField] internal GameObject[] interactableObjects;
        private StateMachine _stateMachine;
        private bool _isInRangeOfPlayer;
        private bool _isInteractable;
        private WaitForSeconds buttonDelay;
    
        public static Action<GameObject[]> onButtonPressed;

        private void Awake()
        {
            _isInteractable = true;
            buttonDelay = new WaitForSeconds(2);
            PlayerInteractionTrigger.onInteractedEvent += OnButtonPressed;
        }

        private void OnButtonPressed(GameObject button)
        {
            if (button == gameObject)
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
            yield return buttonDelay;
            _isInteractable = true;
        }

        private void ButtonPress()
        {
            _isInteractable = false;
            onButtonPressed?.Invoke(interactableObjects);
            StartCoroutine("ActivateButton");
        }
    }
}




