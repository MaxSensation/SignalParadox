//Main author: Maximiliam Rosén

using Interactables.Traps;
using System;
using UnityEngine;

namespace PlayerController
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        public static Action<int> onPlayerTakeDamageEvent;
        public static Action<DamageType> OnPlayerDeath;
        public int CurrentHealth { get; private set; }
        public enum DamageType { Laser, Steam, Bodytrapper, Charger }

        private void Awake()
        {
            SteamController.onSteamDamage += SteamDamage;
            LaserController.onLaserDeath += LaserDamage;
        }

        private void Start()
        {
            CurrentHealth = maxHealth;
        }

        public void SteamDamage(GameObject o)
        {
            if (o != gameObject) return;
            TakeDamage(DamageType.Steam);
        }

        public void BodyTrapperDamage(GameObject o)
        {
            if (o != gameObject) return;
            TakeDamage(DamageType.Bodytrapper);
        }

        public void LaserDamage(GameObject o)
        {
            if (o != gameObject) return;
            CurrentHealth = 1;
            TakeDamage(DamageType.Laser);
        }
        
        public void TakeDamage(DamageType dT)
        {
            CurrentHealth--;
            onPlayerTakeDamageEvent?.Invoke(CurrentHealth);
            if (CurrentHealth <= 0)
                OnPlayerDeath?.Invoke(dT);
            Debug.Log("Damaged Player");
        }

        public void SetHealth(int health)
        {
            CurrentHealth = health;
        }

        private void OnDestroy()
        {
            SteamController.onSteamDamage -= SteamDamage;
            LaserController.onLaserDeath -= LaserDamage;
        }
    }
}
