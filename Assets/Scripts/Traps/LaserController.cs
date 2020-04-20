using AI;
using EventSystem;
using UnityEngine;
using UnityEngine.Events;
using EventHandler = EventSystem.EventHandler;
namespace Traps
{
    public class LaserController : MonoBehaviour
    {
        [SerializeField] private bool laserOn;
        [SerializeField] private LayerMask layermask;
        public UnityEvent turnOn;
        public UnityEvent turnOff;
        private RaycastHit _hit;
        private Collider _lastHit;
        private LineRenderer _lineRenderer;
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _particleSystem = transform.Find("ImpactEffect").GetComponent<ParticleSystem>();

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
        }
    
        private void DeactivateLaser()
        {
            _particleSystem.Stop();
            _lineRenderer.enabled = false;
            laserOn = false;
        }

        private void Update()
        {
            if (laserOn)
            {
                CastLaser();
                CreateLaserEffect();
                CreateLaserImpactEffect();
                CheckForImpact();
                _lastHit = _hit.collider;
            }
        }

        private void CheckForImpact()
        {
            if (_hit.collider)
            {
                var hit = _hit.collider.gameObject;
                if (hit.CompareTag("Player"))
                {
                    EventHandler.InvokeEvent(new OnPlayerDieEvent());
                }

                if (hit.CompareTag("Enemy"))
                {
                    hit.GetComponent<AIController>().Die();
                }
            }
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
            Physics.Raycast(transform.position, transform.forward, out _hit, float.PositiveInfinity, layermask);
        }
    }
}
