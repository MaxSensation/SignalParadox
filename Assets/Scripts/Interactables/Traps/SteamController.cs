//Main author: Maximiliam Rosén

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables.Traps
{
    
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class SteamController : MonoBehaviour
    {
        private enum ColliderType { ParticleSystem, RegularTrigger }
        [SerializeField] [Tooltip("Type of colliderSystem to use")] private ColliderType colliderType;
        [SerializeField] [Tooltip("If on then activate steamLoop")] private bool on;
        [SerializeField] [Tooltip("Declare a interval for the steamLoop")] private string intervalCode;
        [SerializeField] [Tooltip("Delay between each steampuff")] private float steamDelay;
        [SerializeField] [Tooltip("Delay between each damageEvent")] private float damageTickDelay;
        // Instance Events
        public UnityEvent turnOnSteamLoop, turnOffSteamLoop, turnOnSteam, turnOffSteam;
        // Universal Events 
        public static Action<GameObject> onSteamDamage;
        // Particle System
        private ParticleSystem _particleSystem;
        // List of each letter
        private string[] _intervalCodeList;
        // DelayTime
        private WaitForSeconds _steamDelay, _damageTickDelay;
        // Coroutine For SteamLoop
        private Coroutine steamLoop;
        // Variable to check if the steam is on
        // Variable to check if Player can take damage
        private bool _steamIsOn, _canDamagePlayer;

        private void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
            var rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            if (intervalCode.Length > 0)
            {
                // Sub events
                turnOnSteam.AddListener(ActivateSteam);
                turnOffSteam.AddListener(DeactivateSteam);
                turnOnSteamLoop.AddListener(ActivateSteamLoop);
                turnOffSteamLoop.AddListener(DeactivateSteamLoop);
                // Set damage active
                _canDamagePlayer = true;
                // Declare Variables
                _particleSystem = GetComponent<ParticleSystem>();
                // Turn off ParticleSystem
                _particleSystem.Stop();
                // Disable Collision if using RegularTrigger
                if (colliderType == ColliderType.RegularTrigger)
                {
                    var particleSystemCollision = _particleSystem.collision;
                    particleSystemCollision.enabled = false;   
                }
                // Declare delay in Seconds
                _steamDelay = new WaitForSeconds(steamDelay);
                // Declare Damage Tick delay
                _damageTickDelay = new WaitForSeconds(damageTickDelay);
                // Create a list of each code
                _intervalCodeList = intervalCode.Split(',');
                // Turn On Steam if activated
                if(on)turnOnSteamLoop.Invoke();
            }
            // If no SecretCode is declared then Give the user a error message
            else throw new MissingFieldException("SecretCode is empty!");
        }
        
        private void OnDestroy()
        {
            // UnSub events
            turnOnSteam.RemoveListener(ActivateSteam);
            turnOffSteam.RemoveListener(DeactivateSteam);
            turnOnSteamLoop.RemoveListener(ActivateSteamLoop);
            turnOffSteamLoop.RemoveListener(DeactivateSteamLoop);
        }
        
        private void ActivateSteam()
        {
            _steamIsOn = true;
            _particleSystem.Play();
        }

        private void DeactivateSteam()
        {
            _steamIsOn = false;
            _particleSystem.Stop();
        }

        public void ActivateSteamLoop() => steamLoop = StartCoroutine(SteamLoop());
        public void DeactivateSteamLoop()
        {
            turnOffSteam.Invoke();
            StopCoroutine(steamLoop);
        }

        private IEnumerator SteamLoop()
        {
            while (true)
            {
                foreach (var letter in _intervalCodeList)
                {
                    if (letter == "1") turnOnSteam.Invoke(); 
                    else turnOffSteam.Invoke();
                    yield return _steamDelay;
                }
            }
        }
        
        private IEnumerator DamageReset()
        {
            yield return _damageTickDelay;
            _canDamagePlayer = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_steamIsOn) return;
            if (colliderType != ColliderType.RegularTrigger) return;
            if (!other.CompareTag("Player") && !other.CompareTag("Enemy")) return;
            if (!_canDamagePlayer) return;
            onSteamDamage?.Invoke(other.gameObject);
            _canDamagePlayer = false;
            StartCoroutine(DamageReset());
        }

        private void OnParticleCollision(GameObject other)
        {
            if (colliderType != ColliderType.ParticleSystem) return;
            if (!other.CompareTag("Player") && !other.CompareTag("Enemy")) return;
            if (!_canDamagePlayer) return;
            onSteamDamage?.Invoke(other.gameObject);
            _canDamagePlayer = false;
            StartCoroutine(DamageReset());
        }
    }
}
