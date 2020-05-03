using System;
using PlayerController;
using UnityEngine;

namespace Interactables.Pushables
{
    public class PushableBox : MonoBehaviour, IPushable
    {
        private void Awake()
        {
            PlayerInteractionTrigger.onInteractedEvent += HandleInteract;
        }

        private void OnDestroy()
        {
            PlayerInteractionTrigger.onInteractedEvent -= HandleInteract;
        }

        private void HandleInteract(GameObject obj)
        {
            
        }

        public void Push()
        {
            throw new System.NotImplementedException();
        }

        public void GetPushLocation()
        {
            throw new System.NotImplementedException();
        }
    }
}
