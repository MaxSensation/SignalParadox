using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class ThrowDecoyGrenade : MonoBehaviour
{
    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField] private int _maximumThrowableGrenades = 2;
    [SerializeField] private float _throwTargetRange = 20;
    [SerializeField] private float _maxThrowHeight = 5;
    [SerializeField] private float _timeUntilDestroy;
    [SerializeField] private float _gravity = -18;
    private float _oldVerticalRange;
    private bool _shouldDrawPath;
    private bool _canThrow;
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

    public static Action OnAimingEvent;
    public static Action OnOutOfRangeEvent;
    public static Action OnThrowEvent;

    private void Awake()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _lineRenderer.useWorldSpace = true;
        _grenadeRigidBody = _grenadePrefab.GetComponent<Rigidbody>();
        _playerMeshPos = transform.Find("PlayerMesh").transform;
        _cameraPosition = Camera.main.transform;
        _hand = GameObject.Find("mixamorig:RightHand").transform;
        _camera = Camera.main.gameObject;
        _storedLinePoints = new List<Vector3>();
        PickupDecoyGrenade.onGrenadePickup += IncreaseMaxThrowableGrenades;
    }

    private void Update()
    {
        _storedLinePoints.Clear();
        _lineRenderer.positionCount = 0;

        _currentThrowHeight = SetTargetRange();

        if (_shouldDrawPath)
        {
            DrawPath();
        }
    }

    private void IncreaseMaxThrowableGrenades(int pickedUpAmount)
    {
        _maximumThrowableGrenades = pickedUpAmount;
    }

    private IEnumerator DespawnGrenade(Rigidbody thrownGrenade)
    {
        Debug.Log(thrownGrenade.gameObject.name);
        yield return new WaitForSeconds(_timeUntilDestroy);
        thrownGrenade.gameObject.SetActive(false);
        Destroy(thrownGrenade.gameObject);
        _currentThrownGrenades--;
        StopCoroutine("DespawnGrenade");
    }

    public void HandleInput(InputAction.CallbackContext context)
    {
        if (context.started && _currentThrownGrenades < _maximumThrowableGrenades)
        {
            _shouldDrawPath = true;
        }

        if (!context.canceled || _currentThrownGrenades >= _maximumThrowableGrenades) return;
        _shouldDrawPath = false;
        Throw();
    }

    private void Throw()
    {
        if (_canThrow && _currentThrownGrenades < _maximumThrowableGrenades)
        {
            OnThrowEvent?.Invoke();
            _thrownGrenade = Instantiate(_grenadeRigidBody, _hand.position, _hand.rotation);
            Physics.gravity = Vector3.up * _gravity;
            _thrownGrenade.velocity = CalculateLaunchData().initialVelocity;
            _currentThrownGrenades++;
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

            float time = Mathf.Sqrt(-2 * _currentThrowHeight / _gravity) + Mathf.Sqrt(2 * (displacementY - _currentThrowHeight) / _gravity);

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * _gravity * _currentThrowHeight);

            Vector3 velocityXZ = displacementXZ / time;

            return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(_gravity), time);
        }
        else
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

                Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * _gravity * simulationTime * simulationTime / 2f;

                Vector3 drawPoint = _hand.position + displacement;

                previousDrawPoint = drawPoint;
                AddLinePoint(previousDrawPoint);

                _canThrow = true;
            }
        }
        else
        {
            _canThrow = false;
        }
    }

    private float SetTargetRange()
    {
        float _cameraAngle = Vector3.Angle(-_playerMeshPos.transform.up, _camera.transform.forward);
        float currentAngle = _cameraAngle / 150f;
        return currentAngle * _maxThrowHeight;
    }

    private RaycastHit GetTarget()
    {
        LayerMask mask = LayerMask.GetMask("Colliders");
        Physics.Raycast(_cameraPosition.transform.position, _cameraPosition.forward, out RaycastHit hit, _throwTargetRange, mask);
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

    private void AddLinePoint(Vector3 newPoint)
    {
        _storedLinePoints.Add(newPoint);
        _lineRenderer.positionCount = _storedLinePoints.Count;
        _lineRenderer.SetPosition(_storedLinePoints.Count - 1, newPoint);
    }

    private void OnDestroy()
    {
        PickupDecoyGrenade.onGrenadePickup -= IncreaseMaxThrowableGrenades;
    }
}
