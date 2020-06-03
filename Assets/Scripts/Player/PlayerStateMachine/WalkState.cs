//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;

namespace Player.PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/WalkState")]
    public class WalkState : PlayerBaseState
    {
        [SerializeField] private float accelerationSpeed, decelerateSpeed, decelerateThreshold, soundStrength;

        public override void Enter() => Player.Transmitter.SetSoundStrength(soundStrength);

        public override void Run()
        {
            if (IsCharged)
                stateMachine.TransitionTo<ChargedState>();

            // If Player is not grounded then change state to In Air State
            if (!Player.GetRayCast(Vector3.down, GetGroundCheckDistance + GetSkinWidth).collider)
                stateMachine.TransitionTo<InAirState>();
            
            
            // Enter Crouch if Control is pressed 
            if (Player.HasInputCrouch)
                stateMachine.TransitionTo<CrouchState>();
            
            // Get Input direction from user
            var inputVector = Player.GetInputVector(accelerationSpeed);

            // Add Input force to velocity
            Velocity += inputVector;

            // Check if player is moving and if not change state to standState
            if (Velocity.magnitude <= 0)
                stateMachine.TransitionTo<StandState>();

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
        }
    }
}