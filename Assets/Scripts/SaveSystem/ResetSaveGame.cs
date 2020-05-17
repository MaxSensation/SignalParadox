//Main author: Maximiliam Rosén

using UnityEngine;
using UnityEngine.UI;

namespace SaveSystem
{
    public class ResetSaveGame : MonoBehaviour
    {
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

        private static void InvokeLoadLevelEvent()
        {
            SaveManager.SpawnWithFullHealthNextPlayerInstance();
        }
    }
}
