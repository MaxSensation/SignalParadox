//Main author: Andreas Berzelius
//Secondary author: Maximiliam Rosén

using System;
using UnityEngine;

namespace Door
{
    public class AutoCloseTrigger : MonoBehaviour
    {
        public Action onAutoCloseEvent;
        private bool hasClosed;

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player") || hasClosed) return;
            onAutoCloseEvent?.Invoke();
            hasClosed = true;
        }
        internal void SetHasClosed(bool hasClosed) => this.hasClosed = hasClosed;
    }
}
