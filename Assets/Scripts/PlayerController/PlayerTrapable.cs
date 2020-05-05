//Main author: Maximiliam Rosén

using System;
using System.Collections;
using AI.BodyTrapper;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    public class PlayerTrapable : MonoBehaviour
    {
        [SerializeField] private float damageTickTime, minShakeAmount, checkInterval;
        [SerializeField] private int shakeOfAmount;
        private int _currentshakeOfAmount;
        private HealthSystem playerHealthSystem;
        private Transform playerMesh;
        private WaitForSeconds _damageTickTime, _checkInterval;
        private Coroutine _damageOverTime, _mouseShaker;
        private Vector2 _mouseInput;
        private Vector2 _lastMouseInput;
        public static Action<GameObject> onTrapped;
        public static Action onPlayerTrappedEvent;
        public static Action onDetached;
        

        private void Awake()
        {
            _damageTickTime = new WaitForSeconds(damageTickTime);
            _checkInterval = new WaitForSeconds(checkInterval);
            BodyTrapperController.onTrappedPlayer += TrapPlayer;
            playerHealthSystem = GetComponent<HealthSystem>();
            playerMesh = transform.Find("PlayerMesh");
        }
        
        private void OnDestroy()
        {
            BodyTrapperController.onTrappedPlayer -= TrapPlayer;
        }

        private IEnumerator DamageOverTime()
        {
            while (true)
            {
                playerHealthSystem.TakeDamage();
                yield return _damageTickTime;
            }
        }

        private void CheckMouseCheck()
        {
             var newMouseInput= _mouseInput;
             if (_lastMouseInput != null)
             {
                 print(Vector2.Distance(_lastMouseInput, newMouseInput));
                 if (Vector2.Distance(_lastMouseInput, newMouseInput) > minShakeAmount && Vector2.Dot(_lastMouseInput, newMouseInput) < 0.9f)
                     _currentshakeOfAmount++;
                 if (_currentshakeOfAmount >= shakeOfAmount)
                     DetachAllTrappers();
             } 
             _lastMouseInput = _mouseInput;
        }

        private void TrapPlayer(GameObject bodyTrapper)
        {
            bodyTrapper.transform.parent = playerMesh;
            onTrapped?.Invoke(bodyTrapper);
            onPlayerTrappedEvent?.Invoke();
            _damageOverTime = StartCoroutine(DamageOverTime());
            _mouseShaker = StartCoroutine(MouseShaker());
        }

        private IEnumerator MouseShaker()
        {
            while (true)
            {
                CheckMouseCheck();
                yield return _checkInterval;
            }
        }

        private void DetachAllTrappers()
        {
            onDetached?.Invoke();
            StopCoroutine(_mouseShaker);
            StopCoroutine(_damageOverTime);
            _currentshakeOfAmount = 0;
        }

        public void UpdateMouseInput(InputAction.CallbackContext context)
        {
            _mouseInput = context.ReadValue<Vector2>();
        }
    }
}