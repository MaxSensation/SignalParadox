﻿using System.Collections;
using System.Linq;
using EventSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Traps
{
    public class DynamicLaserWall : MonoBehaviour
    {
        [SerializeField] private UnityEvent turnOn;
        [SerializeField] private UnityEvent turnOff;
        [SerializeField] private GameObject laserPrefab;
        [SerializeField] private float laserDensity;
        [SerializeField] private float wallHeight;
        [SerializeField] private bool onStartLaserWallOn;
        [SerializeField] private float delay;
        private LaserController[] _lasers;
        private Transform _laserWallOffset;
        private Transform _laserWallMesh;
        private bool _isLaserOn;
        private void Awake()
        {
            turnOn.AddListener(ActivateLasers);
            turnOff.AddListener(DeactivateLasers);
            _laserWallOffset = transform.Find("PositionOffset");
            _laserWallMesh = _laserWallOffset.Find("LaserWallMesh");
            var localScale = _laserWallMesh.localScale;
            localScale = new Vector3(localScale.x, wallHeight, localScale.z);
            _laserWallMesh.localScale = localScale;
            _laserWallOffset.localPosition = new Vector3(0,wallHeight/2, -0.08f);
            _lasers = new LaserController[(int)(wallHeight/laserDensity)];
            Debug.Log(_lasers.Length);
            GenerateLasers();
        }

        private void Start()
        {
            StartCoroutine("WaitForStart");
            EventHandler.RegisterListener<OnButtonPressedEvent>(OnButtonPressed);
        }

        private void OnButtonPressed(OnButtonPressedEvent obj)
        {
            if (!obj.interactableObjects.Contains(gameObject)) return;
            if (_isLaserOn)
                DeactivateLasers();
            else
                ActivateLasers();
        }

        private void GenerateLasers()
        {
            for (var i = 1; i < (int)(wallHeight/laserDensity) + 1; i++)
            {
                var transform1 = transform;
                var position = transform1.position;
                var laserPos = new Vector3(position.x, position.y + laserDensity * i, position.z);
                var laser = Instantiate(laserPrefab, laserPos, transform1.rotation);
                laser.transform.parent = transform;
                _lasers[i - 1] = laser.GetComponent<LaserController>();
            }
        }

        private IEnumerator WaitForStart()
        {
            yield return new WaitForSeconds(1f);
            if (onStartLaserWallOn)
                ActivateLasers();
            else
                DeactivateLasers();
        }

        private void ActivateLasers()
        {
            _isLaserOn = true;
            StartCoroutine("ActivateWithDelay");
        }

        private void DeactivateLasers()
        {
            _isLaserOn = false;
            StartCoroutine("DeactivateWithDelay");
        }

        private IEnumerator ActivateWithDelay()
        {
            foreach (var l in _lasers)
            {
                l.turnOn.Invoke();
                yield return new WaitForSeconds(delay);
            }

            onStartLaserWallOn = true;
        }
    
        private IEnumerator DeactivateWithDelay()
        {
            for (var index = _lasers.Length - 1; index >= 0; index--)
            {
                var l = _lasers[index];
                l.turnOff.Invoke();
                yield return new WaitForSeconds(delay);
            }

            onStartLaserWallOn = false;
        }
    }
}