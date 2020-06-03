//Main author: Maximiliam Rosén

using System.Collections;
using System.Linq;
using Interactables.Button;
using Interactables.Platform;
using Interactables.Triggers.Platform;
using UnityEngine;
using UnityEngine.Events;

namespace Traps
{
    public class DynamicLaserWall : MonoBehaviour
    {
        [SerializeField] private UnityEvent turnOn, turnOff;
        [SerializeField] private GameObject laserPrefab;
        [SerializeField] private bool onStartLaserWallOn;
        [SerializeField] private float betweenLaserDelay, startDelay, laserDensity, wallHeight;
        [SerializeField] private float distanceFromLaserWall = 0.08f;
        [SerializeField] private Color laserColorStart, laserColorEnd;
        private LaserController[] lasers;
        private Transform laserWallOffset, laserWallMesh;
        private bool isLaserOn, interactable;
        
        private void Awake()
        {
            interactable = true;
            laserWallOffset = transform.Find("PositionOffset");
            laserWallMesh = laserWallOffset.Find("LaserWallMesh");
            var localScale = laserWallMesh.localScale;
            localScale = new Vector3(localScale.x, wallHeight, localScale.z);
            laserWallMesh.localScale = localScale;
            laserWallOffset.localPosition = new Vector3(0,wallHeight/2, -distanceFromLaserWall);
            lasers = new LaserController[(int)(wallHeight/laserDensity)];
            GenerateLasers();
            turnOn.AddListener(ActivateLasers);
            turnOff.AddListener(DeactivateLasers);
            ButtonController.onButtonPressedEvent += OnButtonPressed;
            PlatformTrigger.onButtonPressedEvent += OnButtonPressed;
            StartCoroutine("WaitForStart");
        }

        private void OnDestroy()
        {
            transform.localScale = new Vector3(Berp(0f, 1f, 1f), Berp(0f, 1f, 1f), Berp(0f, 1f, 1f));
            ButtonController.onButtonPressedEvent -= OnButtonPressed;
            PlatformTrigger.onButtonPressedEvent -= OnButtonPressed;
        }

        private float Berp(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }

        private void OnButtonPressed(GameObject[] interactables)
        {
            if (!interactables.Contains(gameObject) || !interactable) return;
            if (isLaserOn)
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
                var laserController = laser.GetComponent<LaserController>();
                laserController.SetColors(laserColorStart, laserColorEnd);
                lasers[i - 1] = laserController;
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

        [ContextMenu("Activate")]
        public void ActivateLasers()
        {
            if (!isLaserOn)
                StartCoroutine("ActivateWithDelay");
        }

        [ContextMenu("Deactivate")]
        public void DeactivateLasers()
        {
            if (isLaserOn)
                StartCoroutine("DeactivateWithDelay");
        }

        private IEnumerator ActivateWithDelay()
        {
            interactable = false;
            foreach (var laser in lasers)
            {
                laser.turnOn.Invoke();
                yield return new WaitForSeconds(betweenLaserDelay);
            }
            isLaserOn = true;
            onStartLaserWallOn = true;
            interactable = true;
        }
    
        private IEnumerator DeactivateWithDelay()
        {
            interactable = false;
            for (var index = lasers.Length - 1; index >= 0; index--)
            {
                var l = lasers[index];
                l.turnOff.Invoke();
                yield return new WaitForSeconds(betweenLaserDelay);
            }
            isLaserOn = false;
            onStartLaserWallOn = false;
            interactable = true;
        }
    }
}
