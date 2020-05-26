//Main author: Andreas Berzelius
//Secondary author: Maximiliam Rosén

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
        [SerializeField] [Tooltip("CurrentState")] private States currentState;
        [SerializeField] [Tooltip("The grenadeProp in the hand")] private Transform grenadeProp;
        private bool shouldDrawPath;
        private Rigidbody thrownGrenade;
        private Transform playerMeshPos, cameraPosition;
        private LineRenderer lineRenderer;
        private float currentThrowHeight;
        private List<Vector3> storedLinePoints;
        private WaitForSeconds despawnTimeSecounds;
        private Coroutine despawnGrenadeCoroutine;
        
        private enum States
        {
            Aiming,
            HoldingGrenade,
            HoldingNoGrenade,
            Occupied,
            Disabled
        }

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
            cameraPosition = Camera.main.transform;


            //Events
            PickupDecoyGrenade.onGrenadePickupEvent += PickupGrenade;
            PlayerTrapable.onDetached += () => currentState = currentState == States.Disabled ? States.Disabled : hasGrenade ? States.HoldingGrenade : States.HoldingNoGrenade;
            PushingState.OnExitPushingStateEvent += () => currentState = currentState == States.Disabled ? States.Disabled : hasGrenade ? States.HoldingGrenade : States.HoldingNoGrenade;
            PushingState.OnEnterPushingStateEvent += () => currentState = States.Occupied;
            PlayerTrapable.onPlayerTrappedEvent += () => currentState = States.Occupied;
            ChargerController.onCrushedPlayerEvent += () => currentState = States.Disabled;
            PlayerAnimatorController.OnDeathAnimBeginning += () => currentState = States.Disabled;
        }
        
        private void OnDestroy()
        {
            PickupDecoyGrenade.onGrenadePickupEvent -= PickupGrenade;
            PlayerTrapable.onDetached -= () => currentState = currentState == States.Disabled ? States.Disabled : hasGrenade ? States.HoldingGrenade : States.HoldingNoGrenade;
            PushingState.OnExitPushingStateEvent -= () => currentState = currentState == States.Disabled ? States.Disabled : hasGrenade ? States.HoldingGrenade : States.HoldingNoGrenade;
            PushingState.OnEnterPushingStateEvent -= () => currentState = States.Occupied;
            PlayerTrapable.onPlayerTrappedEvent -= () => currentState = States.Occupied;
            PlayerAnimatorController.OnDeathAnimBeginning -= () => currentState = States.Disabled;
            ChargerController.onCrushedPlayerEvent -= () => currentState = States.Disabled;
        }

        private void StopAiming()
        {
            shouldDrawPath = false;
            ClearThrowPath();
            onAbortAimEvent?.Invoke();
        }
        
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
        private void PickupGrenade()
        {
            currentState = States.HoldingGrenade;
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
            if (context.started && currentState == States.HoldingGrenade)
            {
                shouldDrawPath = true;
            }
            
            if (context.canceled && currentState == States.Aiming)
                Throw();
        }

        private void Throw()
        {
            shouldDrawPath = false;
            currentState = States.HoldingNoGrenade;
            onThrowEvent?.Invoke();
            if (thrownGrenade != null)
            {
                Destroy(thrownGrenade.gameObject);
                StopCoroutine(despawnGrenadeCoroutine);
            }
            thrownGrenade = Instantiate(grenadePrefabRigidbody, grenadeProp.position, grenadeProp.rotation);
            thrownGrenade.AddForce(Vector3.up * gravity);
            thrownGrenade.velocity = CalculateLaunchData().initialVelocity;
            hasGrenade = false;
            despawnGrenadeCoroutine = StartCoroutine(DespawnGrenade());
        }

        private LaunchData CalculateLaunchData()
        {
            Vector3 newTarget = GetTarget().point;
            if (GetTarget().collider && (newTarget.y - grenadeProp.position.y) < currentThrowHeight)
            {
                float displacementY = newTarget.y - grenadeProp.position.y;
                Vector3 displacementXZ = new Vector3(newTarget.x - grenadeProp.position.x, 0, newTarget.z - grenadeProp.position.z);
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
            if (currentState == States.Disabled || currentState == States.Occupied)
            {
                StopAiming();
                return;
            }
            LaunchData launchData = CalculateLaunchData();
            if (launchData.initialVelocity != Vector3.zero && launchData.timeToTarget != 0f)
            {
                Vector3 previousDrawPoint = grenadeProp.position;
                onAimingEvent?.Invoke();
                for (int i = 0; i <= lineRendererResolution; i++)
                {
                    float simulationTime = i / (float)lineRendererResolution * launchData.timeToTarget;
                    Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
                    Vector3 drawPoint = grenadeProp.position + displacement;
                    previousDrawPoint = drawPoint;
                    AddLinePoint(previousDrawPoint);
                    currentState = States.Aiming;
                }
            }
            else
            {
                currentState = States.HoldingGrenade;
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