﻿//Main author: Maximiliam Rosén

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        if(Cursor.visible)
            Cursor.visible = false;
        SceneManager.LoadScene(levelToLoad);
    }
}
