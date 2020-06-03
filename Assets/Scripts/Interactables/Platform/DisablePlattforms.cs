//Main author: Maximiliam Rosén

using System;
using Interactables.Button;
using UnityEngine;

namespace Interactables.Triggers.Platform
{
    public class DisablePlattforms : MonoBehaviour
    {
        public static Action onDisablePlattformsEvent;
        private bool hasBeenActivated;

        private void Start() => GetComponent<ButtonController>().onStateChangeEvent += Disable;

        private void Disable(ButtonController.ButtonStates state)
        {
            if (state == ButtonController.ButtonStates.Activated && !hasBeenActivated)
                Disable();
        }

        [ContextMenu("Disable All Plattforms")]
        public void Disable()
        {
            hasBeenActivated = true;
            onDisablePlattformsEvent?.Invoke();
        }
    }
}
