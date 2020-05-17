//Main author: Maximiliam Rosén

using System;
using System.Collections;
using AI.BodyTrapper;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerTrapable : MonoBehaviour
    {
        [SerializeField] private float damageTickTime;
        [SerializeField] private float minShakeAmount, checkInterval;
        [SerializeField] private float controllerminShakeAmount;
        [SerializeField] private int shakeOfAmount;
        private int _currentshakeOfAmount;
        private int amountOfTrappedBodyTrappers;
        private HealthSystem playerHealthSystem;
        private Transform playerMesh;
        private WaitForSeconds _damageTickTime, _checkInterval;
        private Coroutine _damageOverTime, _mouseShaker;
        private Vector2 _mouseInput;
        private Vector2 _lastMouseInput;
        public static Action<GameObject> onTrapped;
        public static Action onPlayerTrappedEvent;
        public static Action onDetached;
        private PlayerController _player;
        

        private void Awake()
        {
            _player = GetComponent<PlayerController>();
            _damageTickTime = new WaitForSeconds(damageTickTime);
            _checkInterval = new WaitForSeconds(checkInterval);
            BodyTrapperController.onTrappedPlayer += TrapPlayer;
            BodyTrapperController.onDetachedFromPlayer += DetachFromPlayer;
            playerHealthSystem = GetComponent<HealthSystem>();
            playerMesh = transform.Find("PlayerMesh");
        }

        private void OnDestroy()
        {
            BodyTrapperController.onTrappedPlayer -= TrapPlayer;
            BodyTrapperController.onDetachedFromPlayer -= DetachFromPlayer;
        }

        private IEnumerator DamageOverTime()
        {
            while (_player.isTrapped)
            {
                playerHealthSystem.BodyTrapperDamage(gameObject);
                yield return _damageTickTime;
            }
        }

        private void CheckMouseCheck()
        {
             var newMouseInput= _mouseInput;
             if (_lastMouseInput != null)
             {
                 if (Gamepad.current != null)
                 {
                     if (Vector2.Distance(_lastMouseInput, newMouseInput) > controllerminShakeAmount && Vector2.Dot(_lastMouseInput, newMouseInput) < 0.9f)
                         _currentshakeOfAmount++;
                 }
                 else
                 {
                     if (Vector2.Distance(_lastMouseInput, newMouseInput) > minShakeAmount && Vector2.Dot(_lastMouseInput, newMouseInput) < 0.9f)
                         _currentshakeOfAmount++;
                 }
                 if (_currentshakeOfAmount >= shakeOfAmount)
                     DetachAllTrappers();
             } 
             _lastMouseInput = _mouseInput;
        }

        private void TrapPlayer(GameObject bodyTrapper)
        {
            amountOfTrappedBodyTrappers++;
            bodyTrapper.transform.parent = playerMesh;
            onTrapped?.Invoke(bodyTrapper);
            onPlayerTrappedEvent?.Invoke();
            _damageOverTime = StartCoroutine(DamageOverTime());
            _mouseShaker = StartCoroutine(MouseShaker());
        }

        private IEnumerator MouseShaker()
        {
            while (_player.isTrapped)
            {
                CheckMouseCheck();
                yield return _checkInterval;
            }
        }

        public void DetachAllTrappers()
        {
            onDetached?.Invoke();
            if (_mouseShaker != null)
                StopCoroutine(_mouseShaker);
            if (_damageOverTime != null)
                StopCoroutine(_damageOverTime);
            _currentshakeOfAmount = 0;
        }
        
        private void DetachFromPlayer(GameObject obj)
        {
            amountOfTrappedBodyTrappers--;
            if (amountOfTrappedBodyTrappers == 0)
            {
                StopCoroutine(_mouseShaker);
                StopCoroutine(_damageOverTime);
            }
        }

        public void UpdateMouseInput(InputAction.CallbackContext context)
        {
            _mouseInput = context.ReadValue<Vector2>();
        }
    }
}