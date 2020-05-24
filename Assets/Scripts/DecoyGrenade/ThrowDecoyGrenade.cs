//Main author: Andreas Berzelius

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Player.PlayerStateMachine;
using UnityEngine.InputSystem;
using Player;
using AI.Charger;

[RequireComponent(typeof(Rigidbody), typeof(GameObject))]
public class ThrowDecoyGrenade : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] [Tooltip("Players currrent amount of grenades")] private int currentAmountOfGrenades;
    [SerializeField] [Tooltip("How far can the player throw")] private float throwTargetRange = 20;
    [SerializeField] [Tooltip("How high should the player be able to throw")] private float maxThrowHeight = 5;
    [SerializeField] [Tooltip("Time until grenade despawns")] private float timeUntilDestroy = 10;
    [SerializeField] [Tooltip("The gravity on the thrown grenade")] private float gravity = -9.6f;
    private bool shouldDrawPath, canThrow, IsThrowStopped;
    private int currentThrownGrenades;
    private Rigidbody grenadeRigidBody, thrownGrenade;
    private Transform playerMeshPos, cameraPosition, hand;
    private LineRenderer lineRenderer;
    private float currentThrowHeight;
    private List<Vector3> storedLinePoints;

    public static Action OnPickedUpGrenade;
    //Throw Events
    public static Action OnAimingEvent, OnOutOfRangeEvent, OnThrowEvent;

    private void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        grenadeRigidBody = grenadePrefab.GetComponent<Rigidbody>();
        playerMeshPos = transform.Find("PlayerMesh").transform;
        cameraPosition = Camera.main.transform;
        hand = GameObject.Find("Character1_RightHand").transform;
        storedLinePoints = new List<Vector3>();
        PickupDecoyGrenade.onGrenadePickup += IncreaseMaxThrowableGrenades;
        //Events for when not to throw and to resume Throw
        PushingState.OnEnterPushingStateEvent += StopThrow;
        PushingState.OnExitPushingStateEvent += ResumeThrow;
        PlayerAnimatorController.OnDeathAnimBeginning += StopThrow;
        ChargerController.onCrushedPlayerEvent += StopThrow;
        PlayerTrapable.onPlayerTrappedEvent += StopThrow;
        PlayerTrapable.onDetached += ResumeThrow;
    }

    private void ResumeThrow()
    {
        IsThrowStopped = false;
    }

    private void StopThrow()
    {
        IsThrowStopped = true;
        shouldDrawPath = false;
        canThrow = false;
        OnOutOfRangeEvent?.Invoke();
    }

    private void Update()
    {
        storedLinePoints.Clear();
        //clears the linerenderer
        lineRenderer.positionCount = 0;

        //Updates the throwheight
        currentThrowHeight = SetTargetRange();

        if (shouldDrawPath)
        {
            DrawPath();
        }
    }

    //on pickup increase players current amount of grenades
    private void IncreaseMaxThrowableGrenades()
    {
        if (currentAmountOfGrenades >= 1) return;
        currentAmountOfGrenades = 1;
        OnPickedUpGrenade?.Invoke();
    }

    //Despawn grenade after time
    private IEnumerator DespawnGrenade(Rigidbody thrownGrenade)
    {
        yield return new WaitForSeconds(timeUntilDestroy);
        if (thrownGrenade != null)
        {
            Destroy(thrownGrenade.gameObject);
            currentThrownGrenades--;
        }
    }

    public void HandleInput(InputAction.CallbackContext context)
    {
        //remove the previous grenade if you throw again
        if (context.started && thrownGrenade != null && currentAmountOfGrenades > 0 && !IsThrowStopped)
        {
            Destroy(thrownGrenade.gameObject);
            currentThrownGrenades--;
        }
        if (context.started && currentThrownGrenades < currentAmountOfGrenades && !IsThrowStopped)
        {
            shouldDrawPath = true;
        }
        if (!context.canceled || currentThrownGrenades > currentAmountOfGrenades || IsThrowStopped) return;

        shouldDrawPath = false;
        Throw();
    }

    private void Throw()
    {
        if (canThrow)
        {
            OnThrowEvent?.Invoke();
            thrownGrenade = Instantiate(grenadeRigidBody, hand.position, hand.rotation);
            thrownGrenade.AddForce(Vector3.up * gravity);
            thrownGrenade.velocity = CalculateLaunchData().initialVelocity;
            currentThrownGrenades++;
            currentAmountOfGrenades--;
            StartCoroutine("DespawnGrenade", thrownGrenade);
            canThrow = false;
        }
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
        else //if aiming out of range return a zeroed launchData
        {
            OnOutOfRangeEvent?.Invoke();
            return new LaunchData(Vector3.zero, 0f);
        }
    }

    private void DrawPath()
    {
        LaunchData launchData = CalculateLaunchData();
        Vector3 previousDrawPoint = hand.position;

        int resolution = 30;

        if (launchData.initialVelocity != Vector3.zero && launchData.timeToTarget != 0f)
        {
            OnAimingEvent?.Invoke();

            for (int i = 0; i <= resolution; i++)
            {

                float simulationTime = i / (float)resolution * launchData.timeToTarget;

                Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;

                Vector3 drawPoint = hand.position + displacement;

                previousDrawPoint = drawPoint;
                AddLinePoint(previousDrawPoint);

                canThrow = true;
            }
        }
        else //if launchdata is zeroed you can't throw
        {
            canThrow = false;
        }
    }

    //changes the throw height depending on how high you aim
    private float SetTargetRange()
    {
        float _cameraAngle = Vector3.Angle(-playerMeshPos.transform.up, cameraPosition.forward);
        float currentAngle = _cameraAngle / 150f;
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

    private void OnDestroy()
    {
        PickupDecoyGrenade.onGrenadePickup -= IncreaseMaxThrowableGrenades;
        PushingState.OnEnterPushingStateEvent -= StopThrow;
        PushingState.OnExitPushingStateEvent -= ResumeThrow;
        PlayerAnimatorController.OnDeathAnimBeginning -= StopThrow;
        ChargerController.onCrushedPlayerEvent -= StopThrow;
        PlayerTrapable.onPlayerTrappedEvent -= StopThrow;
        PlayerTrapable.onDetached -= ResumeThrow;
    }

    public int GetCurrentAmountOfGrenades() => currentAmountOfGrenades;
    public void SetCurrentAmountOfGrenades(int amount) => currentAmountOfGrenades = amount;
}