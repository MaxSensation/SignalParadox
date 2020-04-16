using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/CrouchState")]
    public class CrouchState : PlayerBaseState
    {
        [SerializeField] private float accelerationSpeed;
        [SerializeField] private float terminalVelocity;
        [SerializeField] private float decelerateSpeed;
        [SerializeField] private float decelerateThreshold;
        private Vector3 _oldCameraOffset;
        private float _oldColliderHeight;
        
        public override void Enter()
        {
            Debug.Log("Entered Crouch State");
            _oldCameraOffset = CameraOffset;
            CameraOffset = Vector3.zero;
            _oldColliderHeight = PlayerCollider.height;
            PlayerCollider.center = new Vector3(0, -0.35f, 0);
            PlayerCollider.height = 1.1f;
            Player.UpdateCapsuleInfo();
        }
        
        public override void Exit()
        {
            Debug.Log("Exit Crouch State");
            CameraOffset = _oldCameraOffset;
            PlayerCollider.center = Vector3.zero;
            PlayerCollider.height = _oldColliderHeight;
        }

        public override void Run()
        {
            
            // If Player is on Ground and the Player is pressing the jumpKey then change state to JumpState
            if (Player.GetRayCast(Vector3.down, GetGroundCheckDistance + GetSkinWidth).collider && Input.GetKeyDown(KeyCode.Space))
                stateMachine.TransitionTo<JumpState>();

            // // If Player is not grounded then change state to In Air State
            // if (!Player.GetRayCast(Vector3.down, GetGroundCheckDistance + GetSkinWidth).collider)
            //     stateMachine.TransitionTo<InAirState>();
            

            // Enter Crouch if Control is pressed 
            if (Input.GetKey(KeyCode.LeftControl) == false && CanStand())
            {
                stateMachine.TransitionTo<StandState>();
            }

            // Melee attack
            if (Input.GetKeyDown(KeyCode.Mouse0) && Player.hasStunBaton && Player.hasReloaded)
            {
                stateMachine.StackState<CrouchState>();
                stateMachine.TransitionTo<MeleeState>();
            }

            // Fire attack
            if (Input.GetKeyDown(KeyCode.Mouse1) && Player.hasStunGunUpgrade && Player.hasReloaded)
            {
                stateMachine.StackState<CrouchState>();
                stateMachine.TransitionTo<FireState>();
            }

            // Get Input from user
            var inputVector = Player.GetInputVector(accelerationSpeed);

            // Add Input force to velocity
            Velocity += inputVector;
            

            // If any directional inputs accelerate with the accelerateSpeed added with turnSpeed 
            if (inputVector.magnitude > 0) 
                Velocity += Physic3D.GetAcceleration(inputVector, accelerationSpeed + Physic3D.GetTurnVelocity(inputVector, Velocity.normalized));
            else
                Velocity -= Physic3D.GetDeceleration(Velocity, decelerateSpeed, decelerateThreshold);

            LimitVelocity();
        }

        private bool CanStand()
        {
            return !Player.GetRayCast(Vector3.up, 0.5f).collider;
        }

        private void LimitVelocity()
        {
            if (Velocity.magnitude > terminalVelocity) 
                Velocity = Velocity.normalized * terminalVelocity;
        }
    }
}