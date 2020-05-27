//Main author: Maximiliam Rosén

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Traps
{
    public class LaserController : MonoBehaviour
    {
        [SerializeField] private bool laserOn;
        [SerializeField] private LayerMask layermask;
        [SerializeField] private float checkFrequency;
        public UnityEvent turnOn;
        public UnityEvent turnOff;
        private RaycastHit _hit;
        private Collider _lastHit;
        private LineRenderer _lineRenderer;
        private ParticleSystem _particleSystem;
        private WaitForSeconds _checkFrequency;
        private Coroutine laserCoroutine;

        // Events
        public static Action<GameObject> onLaserDeath;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _particleSystem = transform.Find("ImpactEffect").GetComponent<ParticleSystem>();
            _checkFrequency = new WaitForSeconds(checkFrequency);
            if (laserOn)
                ActivateLaser();
            else
                DeactivateLaser();
        }

        private void Start()
        {
            turnOn.AddListener(ActivateLaser);
            turnOff.AddListener(DeactivateLaser);
        }

        private void ActivateLaser()
        {
            _particleSystem.Play();
            _lineRenderer.enabled = true;
            laserOn = true;
            laserCoroutine = StartCoroutine("LaserCoroutine");
        }
    
        private void DeactivateLaser()
        {
            _particleSystem.Stop();
            _lineRenderer.enabled = false;
            laserOn = false;
            if (laserCoroutine != null)
            {
                StopCoroutine(laserCoroutine);
            }
        }

        private IEnumerator LaserCoroutine()
        {
            while (true)
            {
                CastLaser();
                CreateLaserEffect();
                CreateLaserImpactEffect();
                CheckForImpact();
                _lastHit = _hit.collider;
                yield return _checkFrequency;
            }
        }

        private void CheckForImpact()
        {
            if (!_hit.collider) return;
            var hit = _hit.collider.gameObject;
            if (hit.CompareTag("Player") || hit.CompareTag("Enemy"))
                onLaserDeath?.Invoke(hit);
        }

        private void CreateLaserImpactEffect()
        {
            if (_lastHit != _hit.collider)
            {
                _particleSystem.Clear();
            }
            _particleSystem.transform.position = _hit.point;
        }

        private void CreateLaserEffect()
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _hit.point);
        }

        private void CastLaser()
        {
            var tf = transform;
            Physics.Raycast(tf.position, tf.forward, out _hit, float.PositiveInfinity, layermask);
        }

        public void SetColors(Color startColor, Color endColor)
        {
            _lineRenderer.endColor = startColor;
            _lineRenderer.startColor = endColor;
            var gradient = new Gradient();
            gradient = new Gradient();
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;
            colorKey = new GradientColorKey[2];
            colorKey[0].color = startColor;
            colorKey[0].time = 0.0f;
            colorKey[1].color = endColor;
            colorKey[1].time = 1.0f;
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1.0f;
            alphaKey[1].time = 1.0f;
            gradient.SetKeys(colorKey, alphaKey);
            _lineRenderer.colorGradient = gradient;
            var col = _particleSystem.colorOverLifetime;
            col.color = gradient;
        }
    }
}
