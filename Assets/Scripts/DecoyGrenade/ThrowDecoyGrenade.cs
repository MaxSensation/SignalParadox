//Main author: Andreas Berzelius

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlayerController.PlayerStateMachine;
using UnityEngine.InputSystem;
using PlayerController;
using AI.Charger;

[RequireComponent(typeof(Rigidbody), typeof(GameObject))]
public class ThrowDecoyGrenade : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private int currentAmountOfGrenades = 0;
    [SerializeField] private float throwTargetRange = 20;
    [SerializeField] private float maxThrowHeight = 5;
    [SerializeField] private float timeUntilDestroy = 10;
    [SerializeField] private float gravity = -9.6f;
    private bool _shouldDrawPath;
    private bool _canThrow;
    private bool _throwIsStopped;
    private int _currentThrownGrenades;
    private Rigidbody _grenadeRigidBody;
    private LineRenderer _lineRenderer;
    private Transform _playerMeshPos;
    private Transform _cameraPosition;
    private Transform _hand;
    private GameObject _camera;
    private float _currentThrowHeight;
    private List<Vector3> _storedLinePoints;
    private Rigidbody _thrownGrenade;

    //Throw Events
    public static Action OnAimingEvent;
    public static Action OnOutOfRangeEvent;
    public static Action OnThrowEvent;

    private void Awake()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _lineRenderer.useWorldSpace = true;
        _grenadeRigidBody = grenadePrefab.GetComponent<Rigidbody>();
        _playerMeshPos = transform.Find("PlayerMesh").transform;
        _cameraPosition = Camera.main.transform;
        _hand = GameObject.Find("mixamorig:RightHand").transform;
        _camera = Camera.main.gameObject;
        _storedLinePoints = new List<Vector3>();
        PickupDecoyGrenade.onGrenadePickup += IncreaseMaxThrowableGrenades;
        //Events for when not to throw
        PushingState.OnEnterPushingStateEvent += StopThrow;
        PushingState.OnExitPushingStateEvent += ResumeThrow;
        PlayerAnimatorController.OnDeathAnimBeginning += StopThrow;
        ChargerController.onCrushedPlayerEvent += StopThrow;
    }

    private void ResumeThrow()
    {
        _throwIsStopped = false;
    }

    private void StopThrow()
    {
        _throwIsStopped = true;
        _shouldDrawPath = false;
        _canThrow = false;
        OnOutOfRangeEvent?.Invoke();
    }

    private void Update()
    {
        _storedLinePoints.Clear();
        //clears the linerenderer
        _lineRenderer.positionCount = 0;

        //Updates the throwheight
        _currentThrowHeight = SetTargetRange();

        if (_shouldDrawPath)
        {
            DrawPath();
        }
    }

    //on pickup increase players current amount of grenades
    private void IncreaseMaxThrowableGrenades(int pickedUpAmount)
    {
        currentAmountOfGrenades = pickedUpAmount;
    }

    //Despawn grenade after time
    private IEnumerator DespawnGrenade(Rigidbody thrownGrenade)
    {
        yield return new WaitForSeconds(timeUntilDestroy);
        if (thrownGrenade != null)
        {
            Destroy(thrownGrenade.gameObject);
            _currentThrownGrenades--;
        }
    }

    public void HandleInput(InputAction.CallbackContext context)
    {
        //remove the previous grenade if you throw again
        if (context.started && _thrownGrenade != null && currentAmountOfGrenades > 0 && !_throwIsStopped)
        {
            Destroy(_thrownGrenade.gameObject);
            _currentThrownGrenades--;
        }
        if (context.started && _currentThrownGrenades < currentAmountOfGrenades && !_throwIsStopped)
        {
            _shouldDrawPath = true;
        }
        if (!context.canceled || _currentThrownGrenades > currentAmountOfGrenades || _throwIsStopped) return;

        _shouldDrawPath = false;
        Throw();
    }

    private void Throw()
    {
        if (_canThrow)
        {
            OnThrowEvent?.Invoke();
            _thrownGrenade = Instantiate(_grenadeRigidBody, _hand.position, _hand.rotation);
            _thrownGrenade.AddForce(Vector3.up * gravity);
            _thrownGrenade.velocity = CalculateLaunchData().initialVelocity;
            _currentThrownGrenades++;
            currentAmountOfGrenades--;
            StartCoroutine("DespawnGrenade", _thrownGrenade);
        }
    }

    private LaunchData CalculateLaunchData()
    {
        Vector3 _newTarget = GetTarget().point;

        if (GetTarget().collider && (_newTarget.y - _hand.position.y) < _currentThrowHeight)
        {
            float displacementY = _newTarget.y - _hand.position.y;

            Vector3 displacementXZ = new Vector3(_newTarget.x - _hand.position.x, 0, _newTarget.z - _hand.position.z);

            float time = Mathf.Sqrt(-2 * _currentThrowHeight / gravity) + Mathf.Sqrt(2 * (displacementY - _currentThrowHeight) / gravity);

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * _currentThrowHeight);

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
        Vector3 previousDrawPoint = _hand.position;

        int resolution = 30;
        
        if (launchData.initialVelocity != Vector3.zero && launchData.timeToTarget != 0f)
        {
            OnAimingEvent?.Invoke();

            for (int i = 0; i < resolution; i++)
            {

                float simulationTime = i / (float)resolution * launchData.timeToTarget;

                Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;

                Vector3 drawPoint = _hand.position + displacement;

                previousDrawPoint = drawPoint;
                AddLinePoint(previousDrawPoint);

                _canThrow = true;
            }
        }
        else //if launchdata is zeroed you can't throw
        {
            _canThrow = false;
        }
    }

    //changes the throw height depending on how high you aim
    private float SetTargetRange()
    {
        float _cameraAngle = Vector3.Angle(-_playerMeshPos.transform.up, _camera.transform.forward);
        float currentAngle = _cameraAngle / 150f;
        return currentAngle * maxThrowHeight;
    }

    private RaycastHit GetTarget()
    {
        LayerMask mask = LayerMask.GetMask("Colliders");
        Physics.Raycast(_cameraPosition.transform.position, _cameraPosition.forward, out RaycastHit hit, throwTargetRange, mask);
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
        _storedLinePoints.Add(newPoint);
        _lineRenderer.positionCount = _storedLinePoints.Count;
        _lineRenderer.SetPosition(_storedLinePoints.Count - 1, newPoint);
    }

    private void OnDestroy()
    {
        PickupDecoyGrenade.onGrenadePickup -= IncreaseMaxThrowableGrenades;
        PushingState.OnEnterPushingStateEvent -= StopThrow;
        PushingState.OnExitPushingStateEvent -= ResumeThrow;
    }
}