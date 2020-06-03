//Main author: Maximiliam Rosén

using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class SaveGameButton : MonoBehaviour
    {
        private Button button;
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(SaveGame);
        }
        
        private void SaveGame() => SaveManager.SaveGame();
    }
}