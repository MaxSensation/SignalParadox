//Main author: Maximiliam Rosén

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables.Traps
{
    
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    [RequireComponent(typeof(ParticleSystem), typeof(AudioSource))]
    public class SteamController : MonoBehaviour
    {
        [SerializeField] [Tooltip("If on then activate steamLoop")] private bool on;
        [SerializeField] [Tooltip("Declare a interval for the steamLoop")] private string intervalCode;
        [SerializeField] [Tooltip("Delay between each steampuff")] private float steamDelay;
        [SerializeField] [Tooltip("Delay between each damageEvent")] private float damageTickDelay;
        // Instance Events
        public UnityEvent turnOnSteamEvent, turnOffSteamEvent;
        // Universal Events 
        public static Action<GameObject> onSteamDamageEvent;
        // Particle System
        private ParticleSystem steamParticles;
        // List of each letter
        private string[] intervalCodeList;
        // DelayTime
        private WaitForSeconds steamDelaySeconds, damageTickDelaySeconds;
        // Coroutine For SteamLoop
        private Coroutine steamLoop;
        // Variable to check if the steam is on
        // Variable to check if Player can take damage
        private bool isSteamOn, canDamageEntity;

        private void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
            var rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            if (intervalCode.Length > 0)
            {
                // Sub events
                turnOnSteamEvent.AddListener(ActivateSteam);
                turnOffSteamEvent.AddListener(DeactivateSteam);
                // Set damage active
                canDamageEntity = true;
                // Declare Variables
                steamParticles = GetComponent<ParticleSystem>();
                // Turn off ParticleSystem
                steamParticles.Stop();
                // Use a regular trigger
                var particleSystemCollision = steamParticles.collision;
                particleSystemCollision.enabled = false;   
                // Declare delay in Seconds
                steamDelaySeconds = new WaitForSeconds(steamDelay);
                // Declare Damage Tick delay
                damageTickDelaySeconds = new WaitForSeconds(damageTickDelay);
                // Create a list of each code
                intervalCodeList = intervalCode.Split(',');
                // Turn On Steam if activated
                if (on) ActivateSteamLoop();
            }
            // If no SecretCode is declared then Give the user a error message
            else throw new MissingFieldException("SecretCode is empty!");
        }
        
        private void OnDestroy()
        {
            // UnSub events
            turnOnSteamEvent.RemoveListener(ActivateSteam);
            turnOffSteamEvent.RemoveListener(DeactivateSteam);
        }
        
        private void ActivateSteam()
        {
            isSteamOn = true;
            steamParticles.Play();
        }

        private void DeactivateSteam()
        {
            isSteamOn = false;
            steamParticles.Stop();
        }

        public void ActivateSteamLoop() => steamLoop = StartCoroutine(SteamLoop());
        
        public void DeactivateSteamLoop()
        {
            turnOffSteamEvent.Invoke();
            StopCoroutine(steamLoop);
        }

        private IEnumerator SteamLoop()
        {
            while (true)
            {
                foreach (var letter in intervalCodeList)
                {
                    if (letter == "1") turnOnSteamEvent.Invoke(); 
                    else turnOffSteamEvent.Invoke();
                    yield return steamDelaySeconds;
                }
            }
        }
        
        private IEnumerator DamageReset()
        {
            yield return damageTickDelaySeconds;
            canDamageEntity = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!isSteamOn || !canDamageEntity || !other.CompareTag("Player") && !other.CompareTag("Enemy")) return;
            onSteamDamageEvent?.Invoke(other.gameObject);
            canDamageEntity = false;
            StartCoroutine(DamageReset());
        }
    }
}
