using SaveSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Menu
{
    public class SetVolume : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        private Slider slider;
        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.value = SaveManager.Settings.Volume;
        }

        public void SetLevel(float volume)
        {
            SaveManager.Settings.Volume = volume;
            SaveManager.SaveSettings();
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }
    }
}
