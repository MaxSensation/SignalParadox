using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/RunState")]
    public class RunState : PlayerBaseState
    {
        [SerializeField] private float accelerationSpeed;
        // [SerializeField] private float decelerateSpeed;
        // [SerializeField] private float decelerateThreshold;
        
        public override void Enter()
        {
            Debug.Log("Entered Run State");
        }
        
        public override void Run()
        {
            
            // Walk if shift released pressed
            if (Input.GetKeyUp(KeyCode.LeftShift))
                stateMachine.TransitionTo<WalkState>();
            
            // If Player is on Ground and the Player is pressing the jumpKey then change state to JumpState
            if (Player.GetRayCast(Vector3.down, GetGroundCheckDistance + GetSkinWidth).collider && Input.GetKeyDown(KeyCode.Space))
                stateMachine.TransitionTo<JumpState>();

            // If Player is not grounded then change state to In Air State
            if (!Player.GetRayCast(Vector3.down, GetGroundCheckDistance + GetSkinWidth).collider)
                stateMachine.TransitionTo<InAirState>();

            // Enter PushState if E is pressed and interactive 
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Player.SimpleShortRayCast("PushableBox").collider)
                {
                    stateMachine.TransitionTo<PushingState>();    
                }
            }
            
            // Enter Crouch if Control is pressed 
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                stateMachine.TransitionTo<CrouchState>();
            }
            
            // Melee attack
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                stateMachine.StackState<RunState>();
                stateMachine.TransitionTo<MeleeState>();
            }
            
            // Melee attack
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                stateMachine.StackState<RunState>();
                stateMachine.TransitionTo<FireState>();
            }
            
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