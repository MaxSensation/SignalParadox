﻿//Main author: Maximiliam Rosén

using System;
using UnityEngine;

namespace Player.PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/CrouchState")]
    public class CrouchState : PlayerBaseState
    {
        [SerializeField] private float accelerationSpeed;
        [SerializeField] private float terminalVelocity;
        [SerializeField] private float decelerateSpeed;
        [SerializeField] private float decelerateThreshold;
        [SerializeField] private float soundStrength;
        private Vector3 _oldCameraOffset;
        private float _oldColliderHeight;
        private bool _isCrouching;
        
        public static Action onEnteredCrouchEvent;
        public static Action onExitCrouchEvent;

        public override void Enter()
        {
            if (!_isCrouching)
            {
                Player.Transmitter.SetSoundStrength(1 - soundStrength);
                onEnteredCrouchEvent?.Invoke();
                Debug.Log("Entered Crouch State");
                _oldColliderHeight = Player.PlayerCollider.height;
                Player.PlayerCollider.center = new Vector3(0, -0.35f, 0);
                Player.PlayerCollider.height = 1.1f;
                Player.UpdateCapsuleInfo();   
            }
        }
        
        public override void Exit()
        {
            if (!_isCrouching)
            {
                onExitCrouchEvent?.Invoke();
                Debug.Log("Exit Crouch State");
                Player.PlayerCollider.center = Vector3.zero;
                Player.PlayerCollider.height = _oldColliderHeight;
            }
        }

        public override void Run()
        {

            if (Player.GetIsPlayerCharged())
                stateMachine.TransitionTo<ChargedState>();
            

            // Enter Crouch if Control is pressed 
            if (!Player.HasInputCrouch && CanStand())
            {
                _isCrouching = false;
                stateMachine.TransitionTo<StandState>();
            }
            
            // Get Input from user
            var inputVector = Player.GetInputVector(accelerationSpeed);

            // Add Input force to velocity
            Velocity += inputVector;
            
            // If any directional inputs accelerate with the accelerateSpeed added with turnSpeed 
            if (inputVector.magnitude > 0) 
                Velocity += Physic3D.GetAcceleration(inputVector, accelerationSpeed + Physic3D.GetTurnVelocity(inputVector, Velocity.normalized));
            else
            {
                var deceleration = Physic3D.GetDeceleration(Velocity, decelerateSpeed, decelerateThreshold);
                if (deceleration != Vector3.zero)
                    Velocity -= deceleration;
                else
                    Velocity = Vector3.zero;
            }

            LimitVelocity();
        }

        private bool CanStand()
        {
            return !Physics.CapsuleCast(Player.Point1, Player.Point2, Player.PlayerCollider.radius, Vector3.up, 1f, LayerMask.GetMask("Colliders"));
        }

        private void LimitVelocity()
        {
            if (Velocity.magnitude > terminalVelocity) 
                Velocity = Velocity.normalized * terminalVelocity;
        }
    }
}