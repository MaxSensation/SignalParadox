using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyGrenade : MonoBehaviour
{
    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField] private float _throwTargetRange = 20;
    [SerializeField] private float _maxThrowHeight = 5;
    [SerializeField] private float gravity = -18;
    //[SerializeField] private LayerMask _layerMask;
    private bool _shouldDrawPath;
    private bool _canThrow;
    private Rigidbody _grenadeRigidBody;
    private LineRenderer _lineRenderer;
    private Transform _playerMeshPos;
    private Transform _cameraPosition;
    private Transform _hand;
    List<Vector3> _storedLinePoints;

    private void Awake()
    {
        //_lineRenderer = gameObject.GetComponent<LineRenderer>();
        //_lineRenderer.useWorldSpace = true;

        _grenadeRigidBody = _grenadePrefab.GetComponent<Rigidbody>();
        _playerMeshPos = transform.Find("PlayerMesh").transform;
        _cameraPosition = transform.Find("Camera").Find("CameraControls").Find("MainCamera").transform;
        _hand = transform.Find("PlayerMesh").Find("mixamorig:Hips").Find("mixamorig:Spine").Find("mixamorig:Spine1").Find("mixamorig:Spine2").
            Find("mixamorig:RightShoulder").Find("mixamorig:RightArm").Find("mixamorig:RightForeArm").Find("mixamorig:RightHand");
        //Lyssnare för när spelaren trycker på kastknappen
    }

    private void Update()
    {
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
        if (_canThrow)
        {
            _grenadeRigidBody = Instantiate(_grenadeRigidBody, _hand.position, _hand.rotation);
            Physics.gravity = Vector3.up * gravity;
            _grenadeRigidBody.velocity = CalculateLaunchData().initialVelocity;
        }
    }

    private LaunchData CalculateLaunchData()
    {
        Vector3 _newTarget = GetTarget().point;
        Debug.Log(_newTarget.y - _hand.position.y);

        if (GetTarget().collider && (_newTarget.y - _hand.position.y) < _maxThrowHeight)
        {
            float displacementY = _newTarget.y - _hand.position.y;

            Vector3 displacementXZ = new Vector3(_newTarget.x - _hand.position.x, 0, _newTarget.z - _hand.position.z);

            float time = Mathf.Sqrt(-2 * _maxThrowHeight / gravity) + Mathf.Sqrt(2 * (displacementY - _maxThrowHeight) / gravity);

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * _maxThrowHeight);

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

                //if (_lineRenderer != null)
                //    AddLinePoint(drawPoint);

                previousDrawPoint = drawPoint;
                _canThrow = true;
            }
        }
        else
            _canThrow = false;
    }

    private RaycastHit GetTarget()
    {
        LayerMask mask = LayerMask.GetMask("Colliders");
        Physics.Raycast(_cameraPosition.transform.position, _cameraPosition.forward, out RaycastHit hit, _throwTargetRange, mask);
        Debug.Log(Vector3.Dot(-_playerMeshPos.up.normalized, (hit.point - _cameraPosition.position).normalized) /*> maxMinThrowRange*/);

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

    //void AddLinePoint(Vector3 newPoint)
    //{
    //    _storedLinePoints.Add(newPoint); // add the new point to our saved list of line points
    //    _lineRenderer.positionCount = _storedLinePoints.Count + 1; // set the line’s vertex count to how many points we now have, which will be 1 more than it is currently
    //    _lineRenderer.SetPosition(_storedLinePoints.Count - 1, newPoint); // add newPoint as the last point on the line (count -1 because the SetPosition is 0-based and Count is 1-based)    
    //}

    //void RemoveLastLinePoint()
    //{
    //    _storedLinePoints.RemoveAt(_storedLinePoints.Count - 1); // remove the last point from the line
    //    _lineRenderer.positionCount = _storedLinePoints.Count - 1; // set the line’s vertex count to how many points we now have, which will be 1 fewer than it is currently       
    //}

    //private void OnDrawGizmos()
    //{
    //    //LayerMask mask = LayerMask.GetMask("Colliders");
    //    //Physics.Raycast(_cameraPosition.transform.position, _cameraPosition.forward, out RaycastHit hit, _throwTargetRange, mask);
    //    //if (hit.collider)
    //    //{
    //        //Gizmos(Vector3.Dot(-_playerMeshPos.up.normalized, (_cameraPosition.position - hit.point).normalized) /*> maxMinThrowRange*/);
    //        //Gizmos.color = Color.red;
    //        //Gizmos.DrawRay(_playerMeshPos.transform.position, -_playerMeshPos.transform.up);
    //    //}
    //}
}
