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
        [SerializeField] private float controllerMinShakeAmount;
        [SerializeField] private int shakeOfAmount;
        private PlayerController player;
        private HealthSystem playerHealthSystem;
        private Vector2 mouseInput, lastMouseInput;
        private Transform playerMesh;
        private WaitForSeconds damageTickTimeSeconds, checkIntervalSeconds;
        private Coroutine damageOverTime, mouseShaker;
        private int currentShakeOfAmount, amountOfBodyTrappers;
        public static Action<GameObject> onTrappedEvent;
        public static Action onPlayerTrappedEvent, onDetachedEvent;
        

        private void Awake()
        {
            player = GetComponent<PlayerController>();
            playerHealthSystem = GetComponent<HealthSystem>();
            playerMesh = transform.Find("PlayerMesh");
            damageTickTimeSeconds = new WaitForSeconds(damageTickTime);
            checkIntervalSeconds = new WaitForSeconds(checkInterval);
            BodyTrapperController.onTrappedPlayer += AttachBodyTrapper;
            BodyTrapperController.onDetachedFromPlayer += DetachBodyTrapper;
        }

        private void OnDestroy()
        {
            BodyTrapperController.onTrappedPlayer -= AttachBodyTrapper;
            BodyTrapperController.onDetachedFromPlayer -= DetachBodyTrapper;
        }

        private IEnumerator DamageOverTime()
        {
            while (player.IsTrapped)
            {
                playerHealthSystem.BodyTrapperDamage(gameObject);
                yield return damageTickTimeSeconds;
            }
        }

        private void CheckMouseShake()
        {
             var newMouseInput= mouseInput;
             if (lastMouseInput != null)
             {
                 if (Gamepad.current != null)
                 {
                     if (Vector2.Distance(lastMouseInput, newMouseInput) > controllerMinShakeAmount && Vector2.Dot(lastMouseInput, newMouseInput) < 0.9f)
                         currentShakeOfAmount++;
                 }
                 else
                 {
                     if (Vector2.Distance(lastMouseInput, newMouseInput) > minShakeAmount && Vector2.Dot(lastMouseInput, newMouseInput) < 0.9f)
                         currentShakeOfAmount++;
                 }
                 if (currentShakeOfAmount >= shakeOfAmount)
                     DetachAllBodyTrappers();
             } 
             lastMouseInput = mouseInput;
        }

        private void AttachBodyTrapper(GameObject bodyTrapper)
        {
            amountOfBodyTrappers++;
            bodyTrapper.transform.parent = playerMesh;
            onTrappedEvent?.Invoke(bodyTrapper);
            onPlayerTrappedEvent?.Invoke();
            damageOverTime = StartCoroutine(DamageOverTime());
            mouseShaker = StartCoroutine(MouseShaker());
        }

        private void DetachBodyTrapper(GameObject bodyTrapper)
        {
            amountOfBodyTrappers--;
            if (amountOfBodyTrappers != 0) return;
            onDetachedEvent?.Invoke();
            StopCoroutine(mouseShaker);
            StopCoroutine(damageOverTime);
        }

        public void DetachAllBodyTrappers()
        {
            onDetachedEvent?.Invoke();
            if (mouseShaker != null)
                StopCoroutine(mouseShaker);
            if (damageOverTime != null)
                StopCoroutine(damageOverTime);
            currentShakeOfAmount = 0;
        }

        private IEnumerator MouseShaker()
        {
            while (player.IsTrapped)
            {
                CheckMouseShake();
                yield return checkIntervalSeconds;
            }
        }

        public void UpdateMouseInput(InputAction.CallbackContext context) => mouseInput = context.ReadValue<Vector2>();
    }
}