using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/PushingState")]
    public class PushingState : PlayerBaseState
    {
        [SerializeField] private float accelerationSpeed;

        private RaycastHit _box;
        private Vector3 _playerToBoxDirection;
        public override void Enter()
        {
            Debug.Log("Entered Push State");
            _box = Player.SimpleShortRayCast();
            CorrectPlayerPositionForBox();
            _playerToBoxDirection = Vector3.ProjectOnPlane(_box.collider.transform.position - Position, Vector3.up);
            _box.collider.gameObject.transform.parent = Player.transform;
        }

        public override void Exit()
        { ;
            _box.collider.gameObject.transform.parent = null;
        }

        public override void Run()
        {
            // Get Input from user
            var inputVector = GetPushVector();

            // Exit PushState
            if (Input.GetKeyDown(KeyCode.E))
            {
                stateMachine.TransitionTo<StandState>();
            }
            
            // Add Input force to velocity
            Velocity += inputVector;
            
            // If any directional inputs accelerate with the accelerateSpeed added with turnSpeed 
            if (inputVector.magnitude > 0) 
                Velocity += Physic3D.GetAcceleration(inputVector, accelerationSpeed + Physic3D.GetTurnVelocity(inputVector, Velocity.normalized));
            // else
            //     Velocity -= Physic3D.GetDeceleration(Velocity, decelerateSpeed, decelerateThreshold);
        }

        private Vector3 GetPushVector()
        {
            // Get movement input
            var direction = new Vector3(0, 0, Input.GetAxisRaw("Vertical"));
            // If direction is negative then do nothing
            if (direction.z > 0f)
            {
                // If magnitude is greater then 1 normalize the value
                if (direction.magnitude > 1) 
                    return _playerToBoxDirection.normalized * (direction.magnitude * (accelerationSpeed * Time.deltaTime));
                // Else just return the direction
                return _playerToBoxDirection.normalized * (direction.magnitude * (accelerationSpeed * Time.deltaTime));
            }
            return Vector3.zero;
        }
        
        private void CorrectPlayerPositionForBox()
        {
            var distanceFromBoxCenterToEdge = 1f + PlayerCollider.radius;
            var direction = _box.normal;
            var newPosition = _box.collider.bounds.center + distanceFromBoxCenterToEdge * direction;
            Position = new Vector3(newPosition.x, Position.y, newPosition.z);
            Velocity = Vector3.zero;
        }
    }
}