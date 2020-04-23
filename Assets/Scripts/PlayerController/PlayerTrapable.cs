﻿using System;
using AI.BodyTrapper;
using UnityEngine;

namespace PlayerController
{
    public class PlayerTrapable : MonoBehaviour
    {
        private HealthSystem playerHealthSystem;
        private Transform playerMesh;
        public static Action<GameObject> onTrapped;  
        public static Action onDetached;  
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
            onTrapped?.Invoke(bodyTrapper);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                DetachAllTrappers();
            }
        }

        private void DetachAllTrappers()
        {
            onDetached?.Invoke();
        }
    }
}