//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using System.Collections;
using AI.Charger;
using EchoLocation;
using Interactables.Pushables;
using Player.PlayerStateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        // Events
        [Header("PlayerSettings")]
        [SerializeField] [Range(1f, 500f)] private float terminalVelocity;
        [SerializeField] [Range(0f, 10f)] private float dynamicFriction;
        [SerializeField] [Range(0f, 1f)] private float groundCheckDistance;
        [SerializeField] [Range(0f, 1f)] private float skinWidth;
        [SerializeField] [Range(0f, 10f)] private float staticFriction;
        [SerializeField] private LayerMask collisionLayer;
        [SerializeField] private World world;
        public static Action onPlayerDeath;
        public static Action<GameObject> onPlayerInit;
        public State[] states;
        internal CapsuleCollider PlayerCollider { get; private set; }
        internal GameObject PlayerMesh { get; private set; }
        internal Vector3 Point1 { get; private set; }
        internal Vector3 Point2 { get; private set; }
        internal Vector3 CurrentDirection { get; private set; }
        internal SoundProvider Transmitter { get; private set; }
        internal IPushable CurrentPushableObject { get; set; }
        internal bool IsTrapped { get; private set; }
        internal bool HasInputCrouch { get; private set; }
        internal bool EndingPushingState { get; set; }
        internal bool IsPlayerCharged { get; private set; }
        internal Vector3 Velocity { get; set; }
        private bool InCinematic { get; set; }
        private StateMachine stateMachine;
        private Transform cameraTransform;

        private void Awake()
        {
            Transmitter = transform.GetComponentInChildren<SoundProvider>();
            PlayerMesh = transform.Find("PlayerMesh").gameObject;
            stateMachine = new StateMachine(this, states);
            if (Camera.main != null) cameraTransform = Camera.main.transform;
            PlayerCollider = GetComponent<CapsuleCollider>();
            Physic3D.LoadWorldParameters(world);
            ChargerController.onCrushedPlayerEvent += Die;
            ChargerController.CaughtPlayerEvent += PlayerIsCharged;
            PushableBox.onPushStateEvent += HandlePushEvent;
            PlayerTrapable.onPlayerTrappedEvent += EnableTrapped;
            PlayerTrapable.onDetached += DisableTrapped;
            PlayerAnimatorController.OnDeathAnimBeginning += PlayerIsDying;
            PlayerAnimatorController.OnDeathAnimEnd += Die;
        }

        private void OnDestroy()
        {
            ChargerController.onCrushedPlayerEvent -= Die;
            ChargerController.CaughtPlayerEvent -= PlayerIsCharged;
            PushableBox.onPushStateEvent -= HandlePushEvent;
            PlayerTrapable.onPlayerTrappedEvent -= EnableTrapped;
            PlayerTrapable.onDetached -= DisableTrapped;
            PlayerAnimatorController.OnDeathAnimBeginning -= PlayerIsDying;
            PlayerAnimatorController.OnDeathAnimEnd -= Die;
        }

        private void Start()
        {
            onPlayerInit?.Invoke(gameObject);
        }

        private void EnableTrapped()
        {
            EndingPushingState = true;
            IsTrapped = true;
        }

        private void DisableTrapped()
        {
            IsTrapped = false;
        }

        private void HandlePushEvent(IPushable pushable)
        {
            var location = pushable.GetPushLocation(transform.position);
            if (CurrentPushableObject == null && location != Vector3.zero &&
                Vector3.Distance(transform.position, location) < 0.5f && !IsTrapped)
            {
                CurrentPushableObject = pushable;
                stateMachine.TransitionTo<PushingState>();
            }
            else
            {
                EndingPushingState = true;
            }
        }

        private void OnEnable()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // Get CapsuleInfo
            UpdateCapsuleInfo();
            // Run CurrentState
            stateMachine.Run();
            // Add gravity to velocity
            Velocity += Physic3D.GetGravity();
            // Limit the velocity to terminalVelocity
            LimitVelocity();
            // Add Air resistant to the player
            Velocity *= Physic3D.GetAirResistant();
            // Only Move Player as close as possible to the collision
            transform.position += FixCollision();
        }

        public void DebugReset(InputAction.CallbackContext context)
        {
            if (context.performed)
                Die();
        }

        private void PlayerIsDying()
        {
            stateMachine.TransitionTo<DeadState>();
        }

        private void Die()
        {
            Debug.Log("Player Died");
            onPlayerDeath?.Invoke();
        }

        internal void UpdateCapsuleInfo()
        {
            var capsulePosition = transform.position + PlayerCollider.center;
            var distanceToPoints = PlayerCollider.height / 2 - PlayerCollider.radius;
            Point1 = capsulePosition + Vector3.up * distanceToPoints;
            Point2 = capsulePosition + Vector3.down * distanceToPoints;
        }

        private void LimitVelocity()
        {
            // If currentVelocity is greater then terminalVelocity then set the currentVelocity to terminalVelocity
            if (Velocity.magnitude > terminalVelocity)
                Velocity = Velocity.normalized * terminalVelocity;
        }

        private Vector3 FixCollision()
        {
            // Get totalMovement possible per frame
            var movementPerFrame = Velocity * Time.deltaTime;
            while (true)
            {
                // Get hit from CapsuleCast in the direction as Velocity
                var hit = GetRayCast(Velocity.normalized, float.PositiveInfinity);
                // If any collision continue 
                if (!hit.collider) break;
                // If AllowedDistance is greater then MovementPerFrame magnitude continue
                if (hit.distance + skinWidth / Vector3.Dot(movementPerFrame.normalized, hit.normal) >=
                    movementPerFrame.magnitude) break;
                // Get NormalForce
                var normalForce = Physic3D.GetNormalForce(Velocity, hit.normal);
                // Add NormalForce To velocity
                Velocity += normalForce;
                // Add Friction to Velocity
                Velocity = Physic3D.GetFriction(Velocity, normalForce.magnitude, dynamicFriction, staticFriction);
                // Add the new MovementPerFrame
                movementPerFrame = Velocity * Time.deltaTime;
            }

            // Return the possible movement per frame based on collisions
            return movementPerFrame;
        }

        public void UpdateInputVector(InputAction.CallbackContext context)
        {
            if (InCinematic) return;
            var value = context.ReadValue<Vector2>();
            CurrentDirection = new Vector3(value.x, 0, value.y);
        }

        public void OnInputCrouch(InputAction.CallbackContext context)
        {
            HasInputCrouch = context.performed;
        }

        internal Vector3 GetInputVector(float accelerationSpeed)
        {
            // Correct the input based on camera
            var direction = CorrectInputVectorFromCamera(CurrentDirection);
            // If magnitude is greater then 1 normalize the value
            if (direction.magnitude > 1)
                return direction.normalized * (accelerationSpeed * Time.deltaTime);
            // Else just return the direction
            return direction * (accelerationSpeed * Time.deltaTime);
        }

        private Vector3 CorrectInputVectorFromCamera(Vector3 inputVector)
        {
            // Get the horizontal projection velocity
            var projectHorizontal = Vector3.ProjectOnPlane(cameraTransform.rotation * inputVector, Vector3.up);
            // Do a cast in the down direction to check if the player is standing on the ground
            var hit = GetRayCast(Vector3.down, groundCheckDistance + skinWidth);
            // If any collision then project that speed to that normal of that collision else project horizontal only
            return hit.collider
                ? Vector3.ProjectOnPlane(projectHorizontal, hit.normal).normalized
                : projectHorizontal.normalized;
        }

        internal RaycastHit GetRayCast(Vector3 direction, float magnitude)
        {
            // Return a Raycast Hit in the direction and magnitude specific
            Physics.CapsuleCast(Point1, Point2, PlayerCollider.radius, direction.normalized, out var hit, magnitude,
                collisionLayer);
            return hit;
        }

        private void PlayerIsCharged() => IsPlayerCharged = true;

        internal float GetGroundCheckDistance() => groundCheckDistance;

        internal float GetSkinWidth() => skinWidth;

        internal void StartEndingPushingState() => StartCoroutine("EndPushingState");

        private IEnumerator EndPushingState()
        {
            yield return new WaitForSeconds(0.2f);
            stateMachine.TransitionTo<StandState>();
        }
    }
}