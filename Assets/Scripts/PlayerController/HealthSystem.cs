//Main author: Maximiliam Rosén

using System;
using UnityEngine;

namespace PlayerController
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        public static Action<int> onPlayerTakeDamageEvent;
        public int CurrentHealth { get; private set; }

        private void Start()
        {
            CurrentHealth = maxHealth;
        }
        
        public void TakeDamage()
        {
            CurrentHealth--;
            onPlayerTakeDamageEvent?.Invoke(CurrentHealth);
            Debug.Log("Damaged Player");
        }
    }
}
