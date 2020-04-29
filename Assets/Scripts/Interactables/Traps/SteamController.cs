using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Traps
{
    public class SteamController : MonoBehaviour
    {
        [SerializeField] private bool startSteamOn;
        [SerializeField] private string secretCode;
        [SerializeField] private float delay;
        public UnityEvent turnOn;
        public UnityEvent turnOff;
        private ParticleSystem _particleSystem;
        private bool _isRunning;

        public static Action<GameObject> onSteamDeath;

        void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            if (startSteamOn)
                ActivateSteam();
            else
                DeactivateSteam();
            turnOn.AddListener(ActivateSteam);
            turnOff.AddListener(DeactivateSteam);
        }

        private void Update()
        {
            if (secretCode.Length > 0 && !_isRunning)
            {
                StartCoroutine("SecretCodeTimer");
            }
        }

        private IEnumerator SecretCodeTimer()
        {
            _isRunning = true;
            var splitedSecretCode = secretCode.Split(',');
            foreach (var letter in splitedSecretCode)
            {
                if (letter == "1")
                {
                    if (!startSteamOn)
                    {
                        ActivateSteam();
                    }
                }
                else
                {
                    if (startSteamOn)
                    {
                        DeactivateSteam();
                    }
                }
                yield return new WaitForSeconds(delay);
            }
            _isRunning = false;
        }

        private void ActivateSteam()
        {
            _particleSystem.Play();
            startSteamOn = true;
        }
    
        private void DeactivateSteam()
        {
            _particleSystem.Stop();
            startSteamOn = false;
        }

        private void OnParticleCollision(GameObject other)
        {
            if (other)
            {
                if (other.CompareTag("Player") || other.CompareTag("Enemy"))
                {
                    Debug.Log("Hit by Steam");
                    onSteamDeath?.Invoke(other);
                }
            }
        }
    }
}
