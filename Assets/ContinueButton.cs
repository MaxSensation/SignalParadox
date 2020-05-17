using System.Collections;
using System.Collections.Generic;
using Managers;
using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    private Button button;
    private void Start()
    {
        SaveManager.LoadSaveGame();
        if (!SaveManager.HasSaveGame)
            gameObject.SetActive(false);
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadLastCheckpoint);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(LoadLastCheckpoint);
    }

    private void LoadLastCheckpoint()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.DisableFirstLoad();
        SaveManager.LoadCheckPoint();
    }
}
