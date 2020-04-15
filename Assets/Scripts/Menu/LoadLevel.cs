using System;
using EventSystem;
using UnityEngine;
using UnityEngine.UI;
using EventHandler = EventSystem.EventHandler;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private string levelToLoad;
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(InvokeLoadLevelEvent);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(InvokeLoadLevelEvent);
    }

    private void InvokeLoadLevelEvent()
    {
        EventHandler.InvokeEvent(new OnButtonStartEvent(levelToLoad));
    }
}
