using System;
using AI.BodyTrapper;
using UnityEngine;

namespace PlayerController
{
    public class PlayerTrapable : MonoBehaviour
    {
        private HealthSystem playerHealthSystem;
        private Transform playerMesh;
        public static Action<GameObject> onPlayerTrapped;  
        private void Awake()
        {
            BodyTrapperController.onTrappedPlayer += HandleDamage;
            playerHealthSystem = GetComponent<HealthSystem>();
            playerMesh = transform.Find("PlayerMesh");
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
            bodyTrapper.transform.parent = playerMesh;
            onPlayerTrapped?.Invoke(bodyTrapper);
        }
    }
}