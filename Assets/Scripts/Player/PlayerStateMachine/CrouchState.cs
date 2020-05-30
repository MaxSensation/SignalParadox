//Main author: Maximiliam Rosén

using System;
using UnityEngine;

namespace Player.PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/CrouchState")]
    public class CrouchState : PlayerBaseState
    {
        [SerializeField] private float accelerationSpeed, terminalVelocity, decelerateSpeed, decelerateThreshold, soundStrength;
        private float oldColliderHeight;
        private bool isCrouching;
        public static Action onEnteredCrouchEvent, onExitCrouchEvent;

        public override void Enter()
        {
            if (isCrouching) return;
            Player.Transmitter.SetSoundStrength(soundStrength);
            onEnteredCrouchEvent?.Invoke();
            oldColliderHeight = Player.PlayerCollider.height;
            Player.PlayerCollider.center = new Vector3(0, -0.35f, 0);
            Player.PlayerCollider.height = 1.1f;
            Player.UpdateCapsuleInfo();
        }
        
        public override void Exit()
        {
            if (isCrouching) return;
            onExitCrouchEvent?.Invoke();
            Player.PlayerCollider.center = Vector3.zero;
            Player.PlayerCollider.height = oldColliderHeight;
        }

        public override void Run()
        {
            if (IsCharged)
                stateMachine.TransitionTo<ChargedState>();
            
            // Enter Crouch if Control is pressed 
            if (!Player.HasInputCrouch && CanStand())
            {
                isCrouching = false;
                stateMachine.TransitionTo<StandState>();
            }
            
            // Get InputDirection from user
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
            return !Physics.CapsuleCast(
                Player.CapsuleHighPoint, 
                Player.CapsuleLowPoint, 
                Player.PlayerCollider.radius, 
                Vector3.up, 
                1f, 
                LayerMask.GetMask("Colliders")
                );
        }

        private void LimitVelocity()
        {
            if (Velocity.magnitude > terminalVelocity) 
                Velocity = Velocity.normalized * terminalVelocity;
        }
    }
}