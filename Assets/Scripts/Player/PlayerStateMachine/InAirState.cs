//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;

namespace Player.PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/InAirState")]
    public class InAirState : PlayerBaseState
    {
        [SerializeField] private float accelerationSpeed;
        [SerializeField] private float decelerateSpeed;
        [SerializeField] private float decelerateThreshold;
        [SerializeField] private float soundStrength;

        public override void Enter()
        {
            Player._transmitter.SetSoundStrength(1 - soundStrength);
            //Debug.Log("Entered InAir State");
        }
        
        public override void Run()
        {
            if (Player.GetIsPlayerCharged())
                stateMachine.TransitionTo<ChargedState>();

            // If grounded then change to land State
            if (Player.GetRayCast(Vector3.down, GetGroundCheckDistance + GetSkinWidth).collider && Vector3.Dot(Velocity, Vector3.down) > 0.5f)
                stateMachine.TransitionTo<LandState>();
            
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
            }
        }
    }
}