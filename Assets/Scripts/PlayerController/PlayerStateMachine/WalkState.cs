using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/WalkState")]
    public class WalkState : PlayerBaseState
    {
        [SerializeField] private float accelerationSpeed;
        // [SerializeField] private float decelerateSpeed;
        // [SerializeField] private float decelerateThreshold;
        
        public override void Enter()
        {
            Debug.Log("Entered Walk State");
        }
        
        public override void Run()
        {
            // Run if shift is pressed
            if (Input.GetKey(KeyCode.LeftShift))
                stateMachine.TransitionTo<RunState>();
            
            // If Player is on Ground and the Player is pressing the jumpKey then change state to JumpState
            if (Player.GetRayCast(Vector3.down, GetGroundCheckDistance + GetSkinWidth).collider && Input.GetKeyDown(KeyCode.Space))
                stateMachine.TransitionTo<JumpState>();

            // If Player is not grounded then change state to In Air State
            if (!Player.GetRayCast(Vector3.down, GetGroundCheckDistance + GetSkinWidth).collider)
                stateMachine.TransitionTo<InAirState>();

            // Get Input from user
            var inputVector = Player.GetInputVector(accelerationSpeed);

            // Add Input force to velocity
            Velocity += inputVector;

            // Check if player is moving and if not change state to standState
            if (Velocity.magnitude <= 0)
                stateMachine.TransitionTo<StandState>();

            // If any directional inputs accelerate with the accelerateSpeed added with turnSpeed 
            if (inputVector.magnitude > 0) 
                Velocity += Physic3D.GetAcceleration(inputVector, accelerationSpeed + Physic3D.GetTurnVelocity(inputVector, Velocity.normalized));
            // else
            //     Velocity -= Physic3D.GetDeceleration(Velocity, decelerateSpeed, decelerateThreshold);
        }
    }
}