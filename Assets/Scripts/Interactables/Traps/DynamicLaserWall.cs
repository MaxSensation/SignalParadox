//Main author: Maximiliam Rosén

using System.Collections;
using System.Linq;
using Interactables.Button;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables.Traps
{
    public class DynamicLaserWall : MonoBehaviour
    {
        [SerializeField] private UnityEvent turnOn;
        [SerializeField] private UnityEvent turnOff;
        [SerializeField] private GameObject laserPrefab;
        [SerializeField] private float laserDensity;
        [SerializeField] private float wallHeight;
        [SerializeField] private bool onStartLaserWallOn;
        [SerializeField] private float betweenLaserDelay;
        [SerializeField] private float startDelay;
        private LaserController[] _lasers;
        private Transform _laserWallOffset;
        private Transform _laserWallMesh;
        private bool _isLaserOn;
        private bool _interactable;
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
            GenerateLasers();
        }

        private void Start()
        {
            StartCoroutine("WaitForStart");
            ButtonController.onButtonPressed += OnButtonPressed;
            PlatformTrigger.onButtonPressed += OnButtonPressed;
        }

        private void OnDestroy()
        {
            transform.localScale = new Vector3(Berp(0f, 1f, 1f), Berp(0f, 1f, 1f), Berp(0f, 1f, 1f));
            ButtonController.onButtonPressed -= OnButtonPressed;
            PlatformTrigger.onButtonPressed -= OnButtonPressed;
        }
        
        public float Berp(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }

        private void OnButtonPressed(GameObject[] interactables)
        {
            if (!interactables.Contains(gameObject) || !_interactable) return;
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
            yield return new WaitForSeconds(startDelay);
            if (onStartLaserWallOn)
                ActivateLasers();
            else
                DeactivateLasers();
        }

        private void ActivateLasers()
        {
            if (!_isLaserOn)
                StartCoroutine("ActivateWithDelay");
        }

        private void DeactivateLasers()
        {
            if (_isLaserOn)
                StartCoroutine("DeactivateWithDelay");
        }

        private IEnumerator ActivateWithDelay()
        {
            _interactable = false;
            foreach (var l in _lasers)
            {
                l.turnOn.Invoke();
                yield return new WaitForSeconds(betweenLaserDelay);
            }
            _isLaserOn = true;
            onStartLaserWallOn = true;
            _interactable = true;
        }
    
        private IEnumerator DeactivateWithDelay()
        {
            _interactable = false;
            for (var index = _lasers.Length - 1; index >= 0; index--)
            {
                var l = _lasers[index];
                l.turnOff.Invoke();
                yield return new WaitForSeconds(betweenLaserDelay);
            }
            _isLaserOn = false;
            onStartLaserWallOn = false;
            _interactable = true;
        }
    }
}
