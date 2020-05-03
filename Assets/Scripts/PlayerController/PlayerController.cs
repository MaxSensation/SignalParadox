using System;
using AI.Charger;
using Interactables.Pushables;
using PlayerStateMachine;
using Traps;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        public World world;
        public State[] states;
        [Header("PlayerSettings")]
        [SerializeField] [Range(1f, 500f)] private float terminalVelocity;
        [SerializeField] [Range(0f, 1f)] private float staticFriction;
        [SerializeField] [Range(0f, 1f)] private float dynamicFriction;
        [SerializeField] [Range(0f, 1f)] private float skinWidth;
        [SerializeField] [Range(0f, 1f)] private float groundCheckDistance;
        [SerializeField] [Range(0f, 100f)] private float overlayColliderResistant;
        [SerializeField] private LayerMask collisionLayer;
        [SerializeField] private Vector3 velocity;
        private BoxCollider _interactTrigger;
        private StateMachine _stateMachine;
        private CapsuleCollider _collider;
        private Transform _camera;
        private RaycastHit _cameraCast;
        private Vector3 _point1;
        private Vector3 _point2;
        internal GameObject _playerMesh;
        private bool _alive;
        private bool isPlayerCharged;
        internal TurnWithCamera _turnWithCamera; 
        internal SoundProvider _transmitter;
        internal Vector3 currentDirection;
        internal bool hasInputCrouch;
        internal IPushable currentPushableObject;

        // Events
        public static Action onPlayerDeath;

        private void Awake()
        {
            currentDirection = Vector2.zero;
            _transmitter = transform.GetComponentInChildren<SoundProvider>();
            LaserController.onLaserDeath += Die;
            SteamController.onSteamDeath += Die;
            PlayerTrapable.onPlayerTrappedEvent += Die;
            ChargerController.onCrushedPlayerEvent += Die;
            ChargerController.CaughtPlayerEvent += PlayerIsCharged;
            PushableBox.onPushStateEvent += HandlePushEvent;
            _alive = true;
            _playerMesh = GameObject.Find("PlayerMesh");
            _stateMachine = new StateMachine(this, states);
            velocity = Vector3.zero;
            if (Camera.main != null) _camera = Camera.main.transform;
            _collider = GetComponent<CapsuleCollider>();
            Cursor.lockState = CursorLockMode.Locked;
            Physic3D.LoadWorldParameters(world);
            _turnWithCamera = _playerMesh.GetComponent<TurnWithCamera>();
        }

        private void HandlePushEvent(IPushable pushable)
        {
            var location = pushable.GetPushLocation(transform.position);
            if (currentPushableObject == null && location != Vector3.zero && Vector3.Distance(transform.position, location) < 0.5f){
                currentPushableObject = pushable;
                _stateMachine.TransitionTo<PushingState>();
            } else
                _stateMachine.TransitionTo<StandState>();
        }

        private void OnDestroy()
        {
            LaserController.onLaserDeath -= Die;
            SteamController.onSteamDeath -= Die;
            PlayerTrapable.onPlayerTrappedEvent -= Die;
            ChargerController.onCrushedPlayerEvent -= Die;
            ChargerController.CaughtPlayerEvent -= PlayerIsCharged;
            PushableBox.onPushStateEvent -= HandlePushEvent;
        }

        private void Update()
        {
            // Get CapsuleInfo
            UpdateCapsuleInfo();
            // Run CurrentState
            _stateMachine.Run();
            //Ta bort efter spelredovisning
            // Add gravity to velocity
            velocity += Physic3D.GetGravity();
            // Limit the velocity to terminalVelocity
            LimitVelocity();
            // Add Air resistant to the player
            velocity *= Physic3D.GetAirResistant();
            // Fix weird collision clips
            AddOverLayCorrection();
            // Only Move Player as close as possible to the collision
            transform.position += FixCollision();
        }

        public void DebugReset(InputAction.CallbackContext context)
        {
            if (context.performed)
                Die();
        }
        
        private void Die(GameObject o)
        {
            if (gameObject == o && _alive)
            {
                Die();
            }
        }

        private void Die()
        {
            _alive = false;
            Debug.Log("Player Died");
            onPlayerDeath?.Invoke();
        }

        internal void UpdateCapsuleInfo()
        {
            var capsulePosition = transform.position + _collider.center;
            var distanceToPoints = (_collider.height / 2) - _collider.radius;
            _point1 = capsulePosition + Vector3.up * distanceToPoints;
            _point2 = capsulePosition + Vector3.down * distanceToPoints;
        }

        private void LimitVelocity()
        {
            // If currentVelocity is greater then terminalVelocity then set the currentVelocity to terminalVelocity
            if (velocity.magnitude > terminalVelocity)
                velocity = velocity.normalized * terminalVelocity;
        }

        private Vector3 FixCollision()
        {
            // Get totalMovement possible per frame
            var movementPerFrame = velocity * Time.deltaTime;
            while (true)
            {
                // Get hit from CapsuleCast in the direction as Velocity
                var hit = GetRayCast(velocity.normalized, float.PositiveInfinity);
                // If any collision continue 
                if (!hit.collider) break;
                // If AllowedDistance is greater then MovementPerFrame magnitude continue
                if (hit.distance + (skinWidth / Vector3.Dot(movementPerFrame.normalized, hit.normal)) >= movementPerFrame.magnitude) break;
                // Get NormalForce
                var normalForce = Physic3D.GetNormalForce(velocity, hit.normal);
                // Add NormalForce To velocity
                velocity += normalForce;
                // Add Friction to Velocity
                velocity = Physic3D.GetFriction(velocity, normalForce.magnitude, dynamicFriction, staticFriction);
                // Add the new MovementPerFrame
                movementPerFrame = velocity * Time.deltaTime;
            }
            // Return the possible movement per frame based on collisions
            return movementPerFrame;
        }

        public void UpdateInputVector(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            currentDirection = new Vector3(value.x, 0, value.y);
        }

        public void OnInputCrouch(InputAction.CallbackContext context)
        {
            hasInputCrouch = context.performed;
        }

        internal Vector3 GetInputVector(float accelerationSpeed)
        {
            // Correct the input based on camera
            var direction = CorrectInputVectorFromCamera(currentDirection);
            // If magnitude is greater then 1 normalize the value
            if (direction.magnitude > 1)
                return direction.normalized * (accelerationSpeed * Time.deltaTime);
            // Else just return the direction
            return direction * (accelerationSpeed * Time.deltaTime);
        }

        private void AddOverLayCorrection()
        {
            // Get all collides overlapping with the player collider 
            var overlapCollides = Physics.OverlapCapsule(_point1, _point2, _collider.radius, collisionLayer);
            // Loop thru all colliders
            foreach (var overlapCollider in overlapCollides)
            {
                // Get the closest point on the player to the collider
                var playerClosestPointOnBounds = _collider.ClosestPointOnBounds(overlapCollider.transform.position);
                // Get the closest point on the collider to the player
                var colliderOverLapClosestPointOnBounds = overlapCollider.ClosestPointOnBounds(playerClosestPointOnBounds);
                // Add force to the player in the direction from collision point on player to collider
                velocity += -velocity.normalized * ((colliderOverLapClosestPointOnBounds.magnitude + overlayColliderResistant * 100.0f) * Time.deltaTime);
            }
        }

        private Vector3 CorrectInputVectorFromCamera(Vector3 inputVector)
        {
            // Get the horizontal projection velocity
            var projectHorizontal = Vector3.ProjectOnPlane(_camera.rotation * inputVector, Vector3.up);
            // Do a cast in the down direction to check if the player is standing on the ground
            var hit = GetRayCast(Vector3.down, groundCheckDistance + skinWidth);
            // If any collision then project that speed to that normal of that collision else project horizontal only
            return hit.collider ? Vector3.ProjectOnPlane(projectHorizontal, hit.normal).normalized : projectHorizontal.normalized;
        }

        internal RaycastHit GetRayCast(Vector3 direction, float magnitude)
        {
            // Return a Raycast Hit in the direction and magnitude specific
            Physics.CapsuleCast(_point1, _point2, _collider.radius, direction.normalized, out var hit, magnitude, collisionLayer);
            return hit;
        }
        

        internal float GetGroundCheckDistance()
        {
            return groundCheckDistance;
        }

        internal float GetSkinWidth()
        {
            return skinWidth;
        }

        internal Vector3 GetVelocity()
        {
            return velocity;
        }

        internal void SetVelocity(Vector3 velocity)
        {
            this.velocity = velocity;
        }

        internal Vector3 GetPosition()
        {
            return transform.position;
        }

        internal void SetPosition(Vector3 value)
        {
            transform.position = value;
        }

        internal Quaternion GetRotation()
        {
            return _playerMesh.transform.rotation;
        }

        internal void SetRotation(Quaternion value)
        {
            transform.rotation = value;
        }

        internal CapsuleCollider GetPlayerCollider()
        {
            return _collider;
        }

        internal bool GetIsPlayerCharged()
        {
            return isPlayerCharged;
        }

        private void PlayerIsCharged()
        {
            isPlayerCharged = true;
        }
    }
}