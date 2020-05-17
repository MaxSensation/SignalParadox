﻿//Main author: Maximiliam Rosén

using Interactables.Traps;
using System;
using UnityEngine;

namespace Player
{
    public class HealthSystem : MonoBehaviour
    {
        private static readonly int maxHealth = 4;
        public static Action<int> onPlayerTakeDamageEvent;
        public static Action<DamageType> OnPlayerDeath;
        public static Action<int> onInitEvent;
        public int CurrentHealth { get; private set; }
        public enum DamageType { Laser, Steam, Bodytrapper, Charger }

        private void Awake()
        {
            SteamController.onSteamDamage += SteamDamage;
            LaserController.onLaserDeath += LaserDamage;
        }

        private void Start()
        {
            onInitEvent?.Invoke(CurrentHealth);
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
            onInitEvent?.Invoke(CurrentHealth);
        }

        public void ResetHealth()
        {
            CurrentHealth = maxHealth;
            onInitEvent?.Invoke(CurrentHealth);
        }

        private void OnDestroy()
        {
            SteamController.onSteamDamage -= SteamDamage;
            LaserController.onLaserDeath -= LaserDamage;
        }

        public static int GetMaxHP()
        {
            return maxHealth;
        }
    }
}