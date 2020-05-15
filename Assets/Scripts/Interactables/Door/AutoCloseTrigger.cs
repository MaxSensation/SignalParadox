using System;
using UnityEngine;

namespace Interactables.Door
{
    public class AutoCloseTrigger : MonoBehaviour
    {
        public Action onAutoCloseEvent;
        private bool _hasClosed;

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && !_hasClosed)
            {
                onAutoCloseEvent?.Invoke();
                _hasClosed = true;
            }
        }
        internal void SetHasClosed(bool hasClosed) => _hasClosed = hasClosed;
    }
}
