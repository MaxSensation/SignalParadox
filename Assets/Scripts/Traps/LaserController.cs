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
        private RaycastHit hit;
        private Collider lastHit;
        private LineRenderer laserLineRenderer;
        private ParticleSystem impactParticleSystem;
        private WaitForSeconds checkFrequencySeconds;
        private Coroutine laserCoroutine;

        // Events
        public UnityEvent turnOn;
        public UnityEvent turnOff;
        public static Action<GameObject> onLaserDeath;

        private void Awake()
        {
            laserLineRenderer = GetComponent<LineRenderer>();
            impactParticleSystem = transform.Find("ImpactEffect").GetComponent<ParticleSystem>();
            checkFrequencySeconds = new WaitForSeconds(checkFrequency);
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
            impactParticleSystem.Play();
            laserLineRenderer.enabled = true;
            laserOn = true;
            laserCoroutine = StartCoroutine("LaserCoroutine");
        }
    
        private void DeactivateLaser()
        {
            impactParticleSystem.Stop();
            laserLineRenderer.enabled = false;
            laserOn = false;
            if (laserCoroutine != null)
                StopCoroutine(laserCoroutine);
        }

        private IEnumerator LaserCoroutine()
        {
            while (true)
            {
                CastLaser();
                if (lastHit != hit.collider)
                {
                    CheckForImpact();
                    CreateLaserEffect();
                    CreateLaserImpactEffect();
                    lastHit = hit.collider;
                }
                yield return checkFrequencySeconds;
            }
        }

        private void CheckForImpact()
        {
            if (!hit.collider) return;
            var hitCollider = hit.collider.gameObject;
            if (hitCollider.CompareTag("Player") || hitCollider.CompareTag("Enemy"))
                onLaserDeath?.Invoke(hitCollider);
        }

        private void CreateLaserImpactEffect()
        {
            if (lastHit != hit.collider)
                impactParticleSystem.Clear();
            impactParticleSystem.transform.position = hit.point;
        }

        private void CreateLaserEffect()
        {
            laserLineRenderer.SetPosition(0, transform.position);
            laserLineRenderer.SetPosition(1, hit.point);
        }

        private void CastLaser()
        {
            var laser = transform;
            Physics.Raycast(laser.position, laser.forward, out hit, float.PositiveInfinity, layermask);
        }

        public void SetColors(Color startColor, Color endColor)
        {
            laserLineRenderer.endColor = startColor;
            laserLineRenderer.startColor = endColor;
            var gradient = new Gradient();
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
            laserLineRenderer.colorGradient = gradient;
            var col = impactParticleSystem.colorOverLifetime;
            col.color = gradient;
        }
    }
}
