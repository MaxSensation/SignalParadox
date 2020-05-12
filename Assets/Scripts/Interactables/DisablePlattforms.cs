using System;
using UnityEngine;

public class DisablePlattforms : MonoBehaviour
{
    public static Action OnDisablePlattformsEvent;

    [ContextMenu("Disable All Plattforms")]
    public void Disable()
    {
        OnDisablePlattformsEvent?.Invoke();
    }
}
