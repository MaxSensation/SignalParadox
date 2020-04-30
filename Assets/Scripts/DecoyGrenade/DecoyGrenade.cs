using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyGrenade : MonoBehaviour
{
    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField] private Transform _hand;
    [SerializeField] private Transform _cameraPosition;
    [SerializeField] private Vector3 _throwTargetRange;
    [SerializeField] private float h = 25;
    [SerializeField] private float gravity = -18;
    [SerializeField] private LayerMask _layerMask;
    private bool _shouldDrawPath;
    private bool _canThrow;
    private Rigidbody _grenadeRigidBody;
    private LineRenderer _lineRenderer;
    private Transform _playerMeshPos;

    private void Awake()
    {
        //_lineRenderer = _grenadePrefab.GetComponent<LineRenderer>();
        _grenadeRigidBody = _grenadePrefab.GetComponent<Rigidbody>();
        _playerMeshPos = transform.Find("PlayerMesh").transform;
        //Lyssnare för när spelaren trycker på kastknappen
    }

    private void Update()
    {
        //_grenadeRigidBody.position = _hand.position + Vector3.forward;

        if (Input.GetKeyDown(KeyCode.G))
        {
            _shouldDrawPath = true;
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            _shouldDrawPath = false;
            Throw();
        }
        if (_shouldDrawPath)
            DrawPath();

    }

    private void Throw()
    {
        //Debug.Log(CalculateLaunchData().initialVelocity);
        if (_canThrow)
        {
            _grenadeRigidBody = Instantiate(_grenadeRigidBody, _hand.position, _hand.rotation);
            Physics.gravity = Vector3.up * gravity;
            _grenadeRigidBody.velocity = CalculateLaunchData().initialVelocity;
        }
    }

    private LaunchData CalculateLaunchData()
    {
        if (GetTarget().collider)
        {
            Vector3 _newTarget = GetTarget().point;

            float displacementY = _newTarget.y - _hand.position.y;

            Vector3 displacementXZ = new Vector3(_newTarget.x - _hand.position.x, 0, _newTarget.z - _hand.position.z);

            float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);

            Vector3 velocityXZ = displacementXZ / time;

            return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
        }
        else
            return new LaunchData(Vector3.zero, 0f);
    }

    private void DrawPath()
    {
        LaunchData launchData = CalculateLaunchData();
        Vector3 previousDrawPoint = _hand.position;

        int resolution = 30;

        if (launchData.initialVelocity != Vector3.zero || launchData.timeToTarget != 0f)
        {
            for (int i = 1; i <= resolution; i++)
            {

                float simulationTime = i / (float)resolution * launchData.timeToTarget;

                Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;

                Vector3 drawPoint = _hand.position + displacement;

                Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);

                //if(_lineRenderer != null)
                //_lineRenderer.SetPosition(0, drawPoint);

                previousDrawPoint = drawPoint;
                _canThrow = true;
            }
        }
        else
            _canThrow = false;
    }

    private RaycastHit GetTarget()
    {
        Physics.Raycast(_cameraPosition.transform.position, _cameraPosition.forward, out RaycastHit hit, 20f, _layerMask);
        Debug.Log(Vector3.Dot(new Vector3(0, _cameraPosition.position.y, 0).normalized, _playerMeshPos.forward));

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

    private void OnDrawGizmos()
    {
        Physics.Raycast(_hand.position, _cameraPosition.forward, out RaycastHit hit, 20f, _layerMask);
        if (hit.collider)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(gameObject.transform.position, hit.point);
        }
    }
}
