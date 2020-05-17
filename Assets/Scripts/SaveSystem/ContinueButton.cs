using UnityEngine;
using UnityEngine.UI;

namespace SaveSystem
{
    public class ContinueButton : MonoBehaviour
    {
        private Button button;
        private void Start()
        {
            if (!SaveManager.HasSavedGame())
                gameObject.SetActive(false);
            button = GetComponent<Button>();
            button.onClick.AddListener(LoadLastCheckpoint);
        }

        private void OnDestroy() => button.onClick.RemoveListener(LoadLastCheckpoint);

        private static void LoadLastCheckpoint()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            SaveManager.LoadSavedGame();
        }
    }
}
