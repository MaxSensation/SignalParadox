//Main author: Andreas Berzelius

using System;
using System.Collections;
using System.Collections.Generic;
using AI.Charger;
using Player;
using Player.PlayerStateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DecoyGrenade
{
    [RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(LineRenderer))]
    public class ThrowDecoyGrenade : MonoBehaviour
    {
        [SerializeField] [Tooltip("The RigidBody from the grenade prefab")] private Rigidbody grenadePrefabRigidbody;
        [SerializeField] [Tooltip("Player have grenade?")] private bool hasGrenade;
        [SerializeField] [Tooltip("How far can the player throw")] private float throwTargetRange = 20;
        [SerializeField] [Tooltip("How high should the player be able to throw")] private float maxThrowHeight = 5;
        [SerializeField] [Tooltip("Time until grenade despawns")] private float timeUntilDestroy = 10;
        [SerializeField] [Tooltip("The gravity on the thrown grenade")] private float gravity = -9.6f;
        [SerializeField] [Tooltip("The resolution of the lineRenderer")] private int lineRendererResolution = 30;
        private bool shouldDrawPath, canAim, isDisabled;
        private Rigidbody thrownGrenade;
        private Transform playerMeshPos, cameraPosition, hand;
        private LineRenderer lineRenderer;
        private float currentThrowHeight;
        private List<Vector3> storedLinePoints;
        private WaitForSeconds despawnTimeSecounds;
        private Coroutine despawnGrenadeCoroutine;

        public bool HasGrenade
        { 
            get => hasGrenade;
            set => hasGrenade = value;
        }
        
        public static Action onAimingEvent, onAbortAimEvent, onThrowEvent, onPickedUpGrenadeEvent;

        private void Awake()
        {
            despawnTimeSecounds = new WaitForSeconds(timeUntilDestroy);
            storedLinePoints = new List<Vector3>();
            lineRenderer = gameObject.GetComponent<LineRenderer>();
            lineRenderer.useWorldSpace = true;
            playerMeshPos = transform.Find("PlayerMesh").transform;
            hand = GameObject.Find("Character1_RightHand").transform;
            cameraPosition = Camera.main.transform;


            //Events
            PickupDecoyGrenade.onGrenadePickup += IncreaseMaxThrowableGrenades;
            PushingState.OnEnterPushingStateEvent += DisableDecoyGrenade;
            PlayerTrapable.onDetached += EnableAiming;
            PushingState.OnExitPushingStateEvent += EnableDecoyGrenade;
            PlayerAnimatorController.OnDeathAnimBeginning += DisableDecoyGrenade;
            ChargerController.onCrushedPlayerEvent += DisableDecoyGrenade;
            PlayerTrapable.onPlayerTrappedEvent += StopAiming;
        }
        
        private void OnDestroy()
        {
            PickupDecoyGrenade.onGrenadePickup -= IncreaseMaxThrowableGrenades;
            PushingState.OnEnterPushingStateEvent -= StopAiming;
            PushingState.OnExitPushingStateEvent -= EnableAiming;
            PlayerAnimatorController.OnDeathAnimBeginning -= DisableDecoyGrenade;
            ChargerController.onCrushedPlayerEvent -= DisableDecoyGrenade;
            PlayerTrapable.onPlayerTrappedEvent -= StopAiming;
            PlayerTrapable.onDetached -= EnableAiming;
        }

        private void EnableAiming()
        {
            canAim = true;
        } 
        
        private void StopAiming()
        {
            canAim = false;
            shouldDrawPath = false;
            ClearThrowPath();
            onAbortAimEvent?.Invoke();
        }

        private void DisableDecoyGrenade()
        {
            StopAiming();
            isDisabled = true;
        }
        
        private void EnableDecoyGrenade() => isDisabled = false;
        

        private void Update()
        {
            ClearThrowPath();
            
            //Updates the throwheight
            currentThrowHeight = SetTargetRange();
            
            if (shouldDrawPath)
                DrawPath();
        }

        private void ClearThrowPath()
        {
            storedLinePoints.Clear();
            //clears the linerenderer
            lineRenderer.positionCount = 0;
        }

        //on pickup increase players current amount of grenades
        private void IncreaseMaxThrowableGrenades()
        {
            if (hasGrenade) return;
            hasGrenade = true;
            onPickedUpGrenadeEvent?.Invoke();
        }

        //Despawn grenade after time
        private IEnumerator DespawnGrenade()
        {
            yield return despawnTimeSecounds;
            Destroy(thrownGrenade.gameObject);
        }

        public void HandleInput(InputAction.CallbackContext context)
        {
            if (isDisabled) return;
            if (context.started && hasGrenade)
            {
                shouldDrawPath = true;
            }
            if (!context.canceled || !hasGrenade || !canAim) return;
            shouldDrawPath = false;
            Throw();
        }

        private void Throw()
        {
            if (!canAim) return;
            onThrowEvent?.Invoke();
            if (thrownGrenade != null)
            {
                Destroy(thrownGrenade.gameObject);
                StopCoroutine(despawnGrenadeCoroutine);
            }
            thrownGrenade = Instantiate(grenadePrefabRigidbody, hand.position, hand.rotation);
            thrownGrenade.AddForce(Vector3.up * gravity);
            thrownGrenade.velocity = CalculateLaunchData().initialVelocity;
            canAim = false;
            hasGrenade = false;
            despawnGrenadeCoroutine = StartCoroutine(DespawnGrenade());
        }

        private LaunchData CalculateLaunchData()
        {
            Vector3 _newTarget = GetTarget().point;
            if (GetTarget().collider && (_newTarget.y - hand.position.y) < currentThrowHeight)
            {
                float displacementY = _newTarget.y - hand.position.y;
                Vector3 displacementXZ = new Vector3(_newTarget.x - hand.position.x, 0, _newTarget.z - hand.position.z);
                float time = Mathf.Sqrt(-2 * currentThrowHeight / gravity) + Mathf.Sqrt(2 * (displacementY - currentThrowHeight) / gravity);
                Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * currentThrowHeight);
                Vector3 velocityXZ = displacementXZ / time;
                return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
            }
            onAbortAimEvent?.Invoke();
            return new LaunchData(Vector3.zero, 0f);
        }

        private void DrawPath()
        {
            LaunchData launchData = CalculateLaunchData();
            if (launchData.initialVelocity != Vector3.zero && launchData.timeToTarget != 0f)
            {
                Vector3 previousDrawPoint = hand.position;
                onAimingEvent?.Invoke();
                for (int i = 0; i <= lineRendererResolution; i++)
                {
                    float simulationTime = i / (float)lineRendererResolution * launchData.timeToTarget;
                    Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
                    Vector3 drawPoint = hand.position + displacement;
                    previousDrawPoint = drawPoint;
                    AddLinePoint(previousDrawPoint);
                    canAim = true;
                }
            }
            else
            {
                canAim = false;
                shouldDrawPath = false;   
            }
        }

        //changes the throw height depending on how high you aim
        private float SetTargetRange()
        {
            float cameraAngle = Vector3.Angle(-playerMeshPos.transform.up, cameraPosition.forward);
            float currentAngle = cameraAngle / 150f;
            return currentAngle * maxThrowHeight;
        }

        private RaycastHit GetTarget()
        {
            LayerMask mask = LayerMask.GetMask("Colliders");
            Physics.Raycast(cameraPosition.transform.position, cameraPosition.forward, out RaycastHit hit, throwTargetRange, mask);
            return hit;
        }

        private struct LaunchData
        {
            internal readonly Vector3 initialVelocity;
            internal readonly float timeToTarget;

            public LaunchData(Vector3 initialVelocity, float timeToTarget)
            {
                this.initialVelocity = initialVelocity;
                this.timeToTarget = timeToTarget;
            }
        }

        //Stores the points for the linerenderer
        private void AddLinePoint(Vector3 newPoint)
        {
            storedLinePoints.Add(newPoint);
            lineRenderer.positionCount = storedLinePoints.Count;
            lineRenderer.SetPosition(storedLinePoints.Count - 1, newPoint);
        }
    }
}