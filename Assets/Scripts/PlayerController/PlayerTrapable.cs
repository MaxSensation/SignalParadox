using System;
using AI;
using AI.BodyTrapper;
using UnityEngine;

namespace PlayerController
{
    public class PlayerTrapable : MonoBehaviour
    {
        private HealthSystem playerHealthSystem;
        public static Action<GameObject> onPlayerTrapped;  
        private void Awake()
        {
            BodyTrapperController.onTrappedPlayer += HandleDamage;
            playerHealthSystem = GetComponent<HealthSystem>();
        }
        
        private void OnDestroy()
        {
            BodyTrapperController.onTrappedPlayer -= HandleDamage;
        }
    
        private void HandleDamage(GameObject bodyTrapper)
        {
            if (playerHealthSystem.CurrentHealth > 1)
                playerHealthSystem.TakeDamage();
            else
                TrapPlayer(bodyTrapper);
            
        }
    
        private void TrapPlayer(GameObject bodyTrapper)
        {
            bodyTrapper.transform.parent = transform;
            onPlayerTrapped?.Invoke(bodyTrapper);
        }
    }
}