//Main author: Maximiliam Rosén

using System;
using SaveSystem;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameObject gameManager;
        [SerializeField] private AudioMixer audioMixer;

        private void Awake()
        {
            if (gameManager == null)
                gameManager = gameObject;
            if (gameManager != gameObject)
                Destroy(gameObject);
            else
            {
                SaveManager.Init();
                DontDestroyOnLoad(this);
            }
        }

        private void Start()
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(SaveManager.Settings.Volume) * 20);
        }
    }
}