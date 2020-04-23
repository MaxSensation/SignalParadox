using UnityEngine;

namespace PlayerController
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        public int CurrentHealth { get; private set; }

        private void Start()
        {
            CurrentHealth = maxHealth;
        }
        
        public void TakeDamage()
        { 
            CurrentHealth--;
        }
    }
}
