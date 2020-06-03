using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class SetSensitivity : MonoBehaviour
    {
        private Slider slider;
        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.value = SaveManager.Settings.Sensitivity;
        }

        public void SetSens(float sens)
        {
            SaveManager.Settings.Sensitivity = sens;
            SaveManager.SaveSettings();
        }
    }
}
