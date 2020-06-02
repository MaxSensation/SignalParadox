//Main author: Maximiliam Rosén

using System;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    public static Action onMenuOpenEvent, onMenuCloseEvent;
    private void OnEnable()
    {
        onMenuOpenEvent?.Invoke();
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    private void OnDisable()
    {
        onMenuCloseEvent?.Invoke();
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
