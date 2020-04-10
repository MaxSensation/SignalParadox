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
        [SerializeField] private float minDistanceToCenter;
        [SerializeField] private float maxDistanceToCenter;
        [SerializeField] private float maxRotation;
        [SerializeField] private float spreadSpeed;
        [SerializeField] private float projectileForce;
        [SerializeField] private GameObject debrie;
        private BoxCollider triggerZone;
        private CapsuleCollider playerCollider;
        private GameObject leftPatron;
        private GameObject rightPatron;
        private GameObject _leftDebrie;
        private GameObject _rightDebrie;
        private float finalLeftPatronPosition;
        private float finalRightPatronPosition;
        private float finalTriggerSize;
        private Rigidbody _rigidbody;
        private bool isDestoryd;
        private bool _hasCreateDebries;
        
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
            var distanceToCenter = Random.Range(minDistanceToCenter, maxDistanceToCenter);
            triggerZone = GetComponent<BoxCollider>();
            playerCollider = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider>();
            Physics.IgnoreCollision(triggerZone, playerCollider);
            rightPatron = transform.Find("RightPatron").gameObject;
            leftPatron = transform.Find("LeftPatron").gameObject;
            finalLeftPatronPosition = -distanceToCenter;
            finalRightPatronPosition = distanceToCenter;
            finalTriggerSize = distanceToCenter * 2;
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
            yield return new WaitForSeconds(5);
            DestroyEverthing();
        }

        private void DestroyEverthing()
        {
            if (_hasCreateDebries)
            {
                Destroy(_leftDebrie);
                Destroy(_rightDebrie);   
            }
            Destroy(gameObject);
        }

        private void Update()
        {
            if (!isDestoryd)
            {
                // Effect
                CalculatePoints();
                // StunGun
                LerpPatronsAndColider();    
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider)
            {
                if (other.collider.CompareTag("Enemy")){
                    other.collider.GetComponent<AIController>().Die();
                    DestroyEverthing();
                }
                else
                    CreateDebries();
            }
        }

        private void CreateDebries()
        {
            if (!isDestoryd)
            {
                _hasCreateDebries = true;
                isDestoryd = true;
                triggerZone.enabled = false;
                _lineRenderer.enabled = false;
                //LeftDebris
                _leftDebrie = Instantiate(debrie, leftPatron.transform.position, leftPatron.transform.rotation);
                leftPatron.SetActive(false);
                _leftDebrie.GetComponent<Rigidbody>().velocity = _rigidbody.velocity;
                //RightDebris
                _rightDebrie = Instantiate(debrie, rightPatron.transform.position, rightPatron.transform.rotation);
                rightPatron.SetActive(false);
                _rightDebrie.GetComponent<Rigidbody>().velocity = _rigidbody.velocity;   
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
                var leftPosition = leftPatron.transform.position;
                _points[_pointIndexA] = leftPosition;
                var rightPosition = rightPatron.transform.position;
                _points[_pointIndexE] = rightPosition;
                _points[_pointIndexC] = GetCenter(_points[_pointIndexA], _points[_pointIndexE]);
                _points[_pointIndexB] = GetCenter(_points[_pointIndexA], _points[_pointIndexC]);
                _points[_pointIndexD] = GetCenter(_points[_pointIndexC], _points[_pointIndexE]);
                float distance = Vector3.Distance(leftPosition, rightPosition) / _points.Length;
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
