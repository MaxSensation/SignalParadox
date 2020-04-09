using System;
using System.Collections;
using AI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StunGun
{
    public class StunGunPatron : MonoBehaviour
    {
        // StunGun
        [SerializeField] private float maxDistanceToCenter;
        [SerializeField] private float maxRotation;
        [SerializeField] private float spreadSpeed;
        [SerializeField] private float projectileForce;
        private BoxCollider triggerZone;
        private CapsuleCollider playerCollider;
        private GameObject leftPatron;
        private GameObject rightPatron;
        private float finalLeftPatronPosition;
        private float finalRightPatronPosition;
        private float finalTriggerSize;
        private Rigidbody _rigidbody;
        
        // Lighting Effect
        private LineRenderer _lineRenderer;
        private readonly int _pointsCount = 5;
        private readonly int _half = 2;
        private float _randomness;
        private Vector3[] _points;
        private readonly int _pointIndexA = 0;
        private readonly int _pointIndexB = 1;
        private readonly int _pointIndexC = 2;
        private readonly int _pointIndexD = 3;
        private readonly int _pointIndexE = 4;
        private readonly string _mainTexture = "_MainTex";
        private Vector2 _mainTextureScale = Vector2.one;
        private Vector2 _mainTextureOffset = Vector2.one;
        private float _timer;
        private float _timerTimeOut = 0.05f;
    
        private void Awake()
        {
            // StunGun
            triggerZone = GetComponent<BoxCollider>();
            playerCollider = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider>();
            Physics.IgnoreCollision(triggerZone, playerCollider);
            rightPatron = transform.Find("RightPatron").gameObject;
            leftPatron = transform.Find("LeftPatron").gameObject;
            finalLeftPatronPosition = -maxDistanceToCenter;
            finalRightPatronPosition = maxDistanceToCenter;
            finalTriggerSize = maxDistanceToCenter * 2;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.AddForce(transform.forward.normalized * projectileForce + Vector3.up * 10);
            _rigidbody.AddTorque(transform.forward * Random.Range(-maxRotation, maxRotation));
            _rigidbody.AddTorque(transform.up * Random.Range(-maxRotation/4, maxRotation/4));
            // Effect
            _lineRenderer = GetComponent<LineRenderer>();
            _points = new Vector3[_pointsCount];
            _lineRenderer.positionCount = _pointsCount;
            StartCoroutine("ActivateDestructionTimer");
        }

        private IEnumerator ActivateDestructionTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }

        private void Update()
        {
            // Effect
            CalculatePoints();
            // StunGun
            LerpPatronsAndColider();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider)
            {
                if (other.collider.CompareTag("Enemy"))
                    other.collider.GetComponent<AIController>().Die();
                Destroy(gameObject);
            }
        }

        private void LerpPatronsAndColider()
        {
            leftPatron.transform.localPosition = new Vector3(Mathf.Lerp(leftPatron.transform.localPosition.x, finalLeftPatronPosition, spreadSpeed), 0, 0);
            rightPatron.transform.localPosition = new Vector3(Mathf.Lerp(rightPatron.transform.localPosition.x, finalRightPatronPosition, spreadSpeed), 0, 0);
            triggerZone.size = new Vector3(Mathf.Lerp(triggerZone.size.x, finalTriggerSize, spreadSpeed), triggerZone.size.y, triggerZone.size.z);
        }

        private void CalculatePoints()
        {
            _timer += Time.deltaTime;

            if (_timer > _timerTimeOut)
            {
                _timer = 0;

                _points[_pointIndexA] = leftPatron.transform.position;
                _points[_pointIndexE] = rightPatron.transform.position;
                _points[_pointIndexC] = GetCenter(_points[_pointIndexA], _points[_pointIndexE]);
                _points[_pointIndexB] = GetCenter(_points[_pointIndexA], _points[_pointIndexC]);
                _points[_pointIndexD] = GetCenter(_points[_pointIndexC], _points[_pointIndexE]);

                float distance = Vector3.Distance(leftPatron.transform.position, rightPatron.transform.position) / _points.Length;
                _mainTextureScale.x = distance;
                _mainTextureOffset.x = Random.Range(-_randomness, _randomness);
                _lineRenderer.material.SetTextureScale(_mainTexture, _mainTextureScale);
                _lineRenderer.material.SetTextureOffset(_mainTexture, _mainTextureOffset);

                _randomness = distance / (_pointsCount * _half);

                SetRandomness();

                _lineRenderer.SetPositions(_points);
            }
        }
    
        private void SetRandomness()
        {
            for (int i = 0; i < _points.Length; i++)
            {
                if (i != _pointIndexA && i != _pointIndexE)
                {
                    _points[i].x += Random.Range(-_randomness, _randomness);
                    _points[i].y += Random.Range(-_randomness, _randomness);
                    _points[i].z += Random.Range(-_randomness, _randomness);
                }
            }
        }
    
        private Vector3 GetCenter(Vector3 a, Vector3 b)
        {
            return (a + b) / _half;
        }
    }
}
