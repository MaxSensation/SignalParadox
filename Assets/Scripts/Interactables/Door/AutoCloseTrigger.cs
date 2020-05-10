using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCloseTrigger : MonoBehaviour
{
    public Action OnAutoClose;
    private bool _hasClosed;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !_hasClosed)
        {
            OnAutoClose?.Invoke();
            _hasClosed = true;
        }
    }
    internal void SetHasClosed(bool hasClosed) => _hasClosed = hasClosed;
}
