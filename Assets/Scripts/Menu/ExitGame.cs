using System;
using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{

    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(EndGame);
    }

    private void EndGame()
    {
        print("Exited Game");
        Application.Quit();
    }
}
