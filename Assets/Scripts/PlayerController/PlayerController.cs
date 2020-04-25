using System;
using System.Collections;
using AI;
using AI.Charger;
using Pickups;
using Traps;
using UnityEngine;

namespace PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        public World world;
        public State[] states;
        [SerializeField] [Range(1f, 500f)] private float terminalVelocity;
        [SerializeField] [Range(0f, 1f)] private float staticFriction;
        [SerializeField] [Range(0f, 1f)] private float dynamicFriction;
        [SerializeField] [Range(0f, 1f)] private float skinWidth;
        [SerializeField] [Range(0f, 1f)] private float groundCheckDistance;
        [SerializeField] [Range(0f, 100f)] private float overlayColliderResistant;
        [SerializeField] [Range(0f, 500f)] private float mouseSensitivity;
        [SerializeField] private bool thirdPersonCamera;
        [SerializeField] private float thirdPersonCameraSize;
        [SerializeField] private float thirdPersonCameraMaxAngle;
        [SerializeField] private float thirdPersonOffsetHorizontal;
        [SerializeField] private float thirdPersonCameraDistance;
        [SerializeField] internal bool hasReloaded;
        [SerializeField] private LayerMask collisionLayer;
        [SerializeField] internal bool hasStunBaton;
        [SerializeField] internal bool hasStunGunUpgrade;
        [SerializeField] private Vector3 velocity;
        [SerializeField] private LayerMask _layermask;
        private BoxCollider _interactTrigger;
        private StateMachine _stateMachine;
        private CapsuleCollider _collider;
        private Transform _camera;
        private Vector2 _cameraRotation;
        private RaycastHit _cameraCast;
        private Vector3 _cameraOffset;
        private Vector3 _point1;
        private Vector3 _point2;
        private GameObject _playerMesh;
        private bool _alive;
        private bool isPlayerCharged;

        // Events
        public static Action onPlayerDeath;
        public static Action OnMeleeEvent;


        private void Awake()
        {
            LaserController.onLaserDeath += Die;
            SteamController.onSteamDeath += Die;
            ChargerController.onCrushedPlayerEvent += Die;
            ChargerController.CaughtPlayerEvent += PlayerIsCharged;
            PickupStunBaton.onStunBatonPickup += EnableStunBaton;
            PickupStunGunUpgrade.onStunGunUpgradePickup += EnableStunGun;
            _alive = true;
            _playerMesh = GameObject.Find("PlayerMesh");
            _stateMachine = new StateMachine(this, states);
            terminalVelocity = 7f;
            staticFriction = 0.6f;
            dynamicFriction = 0.3f;
            skinWidth = 0.05f;
            groundCheckDistance = 0.05f;
            overlayColliderResistant = 20f;
            mouseSensitivity = 100;
            velocity = Vector3.zero;
            thirdPersonCamera = true;
            thirdPersonCameraDistance = 2f;
            thirdPersonOffsetHorizontal = 0.5f;
            thirdPersonCameraSize = 0.5f;
            thirdPersonCameraMaxAngle = 25f;
            if (Camera.main != null) _camera = Camera.main.transform;
            _collider = GetComponent<CapsuleCollider>();
            _cameraRotation = new Vector2(_playerMesh.transform.rotation.eulerAngles.y,0);
            _cameraOffset = _camera.localPosition;
            Cursor.lockState = CursorLockMode.Locked;
            Physic3D.LoadWorldParameters(world);
            hasReloaded = true;
        }

        private void OnDestroy()
        {
            LaserController.onLaserDeath -= Die;
            SteamController.onSteamDeath -= Die;
            ChargerController.onCrushedPlayerEvent -= Die;
            ChargerController.CaughtPlayerEvent -= PlayerIsCharged;
            PickupStunBaton.onStunBatonPickup -= EnableStunBaton;
            PickupStunGunUpgrade.onStunGunUpgradePickup -= EnableStunGun;
        }

        private void Update()
        {
            // Get CapsuleInfo
            UpdateCapsuleInfo();
            // Rotate PlayerMesh
            RotatePlayerMesh();
            // Run CurrentState
            _stateMachine.Run();
            //CheckForButtonAfterPush
            // TryPushButton();
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
            // RotateCamera from player input
            RotateCamera();
            // Move Camera based on thirdPerson or firstPerson
            MoveCamera();
        }

        internal void OnMelee()
        {
            OnMeleeEvent?.Invoke();
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
        
        private IEnumerator ReloadTime()
        {
            yield return new WaitForSeconds(2f);
            hasReloaded = true;
            StopCoroutine("ReloadTime");
        }

        internal void Reloading()
        {
            StartCoroutine("ReloadTime");
        }

        private void RotatePlayerMesh()
        {
            _playerMesh.transform.rotation = Quaternion.Euler(0, _cameraRotation.x, 0);
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

        internal Vector3 GetInputVector(float accelerationSpeed)
        {
            // Get movement input
            var direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            // Correct the input based on camera
            direction = CorrectInputVectorFromCamera(direction);
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

        private void RotateCamera()
        {
            // Get rawAxis rotation from the mouse
            var cameraRotation = new Vector2(Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X")) * (mouseSensitivity * Time.deltaTime);
            // Rotate the camera on x
            _cameraRotation.x += cameraRotation.y;
            // Rotate the camera on y
            _cameraRotation.y -= cameraRotation.x;
            // Limit the y rotation to lowest and highest point
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, thirdPersonCamera ? -thirdPersonCameraMaxAngle : -89.9f, 89.9f);
            // Update the rotation to the camera
            _camera.localRotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0);
        }

        private void MoveCamera()
        {
            // If in Third Person then update the position of the camera related to the player
            if (thirdPersonCamera)
            {
                // Save the players position
                var playerPosition = transform.position + _cameraOffset + (_playerMesh.transform.right * thirdPersonOffsetHorizontal);
                // Get the position the Camera want to move to
                var whereCameraWantToMove = (playerPosition) - (_camera.forward * (thirdPersonCameraDistance - thirdPersonCameraSize));
                // Get the direction from the players First Person Camera Position to the position where the Third Person Camera position want to move
                var direction = (whereCameraWantToMove - playerPosition).normalized;
                // Get the distance from the players First Person Camera Position to the position where the Third Person Camera position want to move
                var distance = (whereCameraWantToMove - playerPosition).magnitude;
                // Get the position from the players First Person Camera Position to Third Person Camera position want to move and move the camera to that position based on collisions
                var newCameraPos = Physics.SphereCast(playerPosition, thirdPersonCameraSize, direction, out _cameraCast, distance) ? playerPosition + direction * (_cameraCast.distance - thirdPersonCameraSize) : whereCameraWantToMove;
                if ((_cameraCast.collider && _cameraCast.distance < 0.4f) || Physics.OverlapSphere(newCameraPos, thirdPersonCameraSize, collisionLayer).Length > 0)
                {
                    _camera.localPosition = _cameraOffset;
                }
                else
                {
                    _camera.position = newCameraPos;
                }
            }
            // If in First Person then update the position to zero 
            else _camera.localPosition = _cameraOffset;
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

        internal RaycastHit GetMeeleRayCast(float meeleDistance) //Temporär kanske, för den e bah för meele justn nu
        {
            Physics.CapsuleCast(_point1, _point2, _collider.radius, _playerMesh.transform.forward, out var hit, meeleDistance);
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

        public void AddForce(Vector3 force)
        {
            velocity += force;
        }

        internal RaycastHit SimpleShortRayCast()
        {
            Physics.Raycast(transform.position, _playerMesh.transform.forward, out var hit, 1f, _layermask, QueryTriggerInteraction.Ignore);
            return hit;
        }

        internal bool CheckSimpleShortRayCast(string tagName)
        {
            Physics.Raycast(transform.position, _playerMesh.transform.forward, out var hit, 1f, _layermask, QueryTriggerInteraction.Ignore);
            return hit.collider && hit.collider.CompareTag(tagName);
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

        internal Vector3 GetCameraOffset()
        {
            return _cameraOffset;
        }

        internal void SetCameraOffset(Vector3 value)
        {
            _cameraOffset = value;
        }

        internal Vector3 GetPlayerCameraDirection()
        {
            return _playerMesh.transform.forward;
        }

        internal bool GetIsPlayerCharged()
        {
            return isPlayerCharged;
        }

        private void PlayerIsCharged()
        {
            isPlayerCharged = true;
        }

        public Vector2 GetCameraRotation()
        {
            return _cameraRotation;
        }
    
        private void EnableStunBaton()
        {
            hasStunBaton = true;
        }
    
        private void EnableStunGun()
        {
            hasStunGunUpgrade = true;
        }
    }
}