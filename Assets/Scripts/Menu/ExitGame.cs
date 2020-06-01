//Main author: Maximiliam Rosén

using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(EndGame);
    }

    private void EndGame()
    {
        SaveManager.SaveGame();
        Application.Quit();
    }
}
