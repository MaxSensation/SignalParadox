//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using Interactables.DecoyGrenade;
using Interactables.Triggers.EntitiesTrigger;
using Player.PlayerStateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        [SerializeField] private float smoothTime;
        private Animator playerAnimator;
        private GameObject currentPickedUpDecoy;
        private bool hasDecoy;
        private Vector2 movement, newMovement;
        public static Action OnDeathAnimBeginningEvent, OnDeathAnimEndEvent;
        public static Action<GameObject> onTouchedGrenadeEvent;

        private void Awake()
        {
            playerAnimator = GetComponent<Animator>();
            CrouchState.onEnteredCrouchEvent += EnteredCrouch;
            CrouchState.onExitCrouchEvent += ExitedCrouch;
            ThrowDecoyGrenade.onAimingEvent += Aiming;
            ThrowDecoyGrenade.onAbortAimEvent += StopAiming;
            ThrowDecoyGrenade.onThrowEvent += Throw;
            InteractionTrigger.onInteractedEvent += OnPickedUpDecoy;
            PushingState.onEnterPushingStateEvent += HandleEnterPushing;
            PushingState.onExitPushingStateEvent += HandleExitPushing;
            PushingState.onPushingStateEvent += HandlePushing;
            HealthSystem.onPlayerDeathEvent += PlayerDeath;
            PlayerTrapable.onPlayerTrappedEvent += OnTrapped;
            PlayerTrapable.onDetachedEvent += OnDetached;
        }

        private void OnDestroy()
        {
            CrouchState.onEnteredCrouchEvent -= EnteredCrouch;
            CrouchState.onExitCrouchEvent -= ExitedCrouch;
            ThrowDecoyGrenade.onAimingEvent -= Aiming;
            ThrowDecoyGrenade.onThrowEvent -= Throw;
            ThrowDecoyGrenade.onAbortAimEvent -= StopAiming;
            InteractionTrigger.onInteractedEvent -= OnPickedUpDecoy;
            PushingState.onEnterPushingStateEvent -= HandleEnterPushing;
            PushingState.onExitPushingStateEvent -= HandleExitPushing;
            PushingState.onPushingStateEvent -= HandlePushing;
            HealthSystem.onPlayerDeathEvent -= PlayerDeath;
            PlayerTrapable.onPlayerTrappedEvent -= OnTrapped;
            PlayerTrapable.onDetachedEvent -= OnDetached;
        }

        private void Update()
        {
            movement = Vector2.Lerp(movement, newMovement, Time.deltaTime * smoothTime);
            playerAnimator.SetFloat("Vertical", movement.y);
            playerAnimator.SetFloat("Horizontal", movement.x);
        }

        public void UpdateMovementInput(InputAction.CallbackContext context) => newMovement = context.ReadValue<Vector2>();

        private void PlayerDeath(HealthSystem.DamageType dType)
        {
            switch (dType)
            {
                case HealthSystem.DamageType.Laser:
                    playerAnimator.SetTrigger("LaserDeath");
                    DeathAnimBeginning();
                    break;
                case HealthSystem.DamageType.Steam:
                    playerAnimator.SetTrigger("GasDeath");
                    DeathAnimBeginning();
                    break;
                case HealthSystem.DamageType.Bodytrapper:
                    playerAnimator.SetTrigger("GasDeath");
                    DeathAnimBeginning();
                    break;
                case HealthSystem.DamageType.Charger:
                    break;
                default:
                    break;
            }
        }

        private void DeathAnimEnd() => OnDeathAnimEndEvent?.Invoke();

        private void ActivateDecoyProp() => onTouchedGrenadeEvent?.Invoke(currentPickedUpDecoy);

        private void DeathAnimBeginning() => OnDeathAnimBeginningEvent?.Invoke();

        private void HandlePushing(bool pushing) => playerAnimator.SetBool("Pushing", pushing);

        private void HandleEnterPushing() => playerAnimator.SetBool("EnteredPushing", true);

        private void HandleExitPushing() => playerAnimator.SetBool("EnteredPushing", false);


        private void Aiming()
        {
            playerAnimator.SetBool("Aim", true);
            playerAnimator.SetBool("Throw", false);
        }

        private void StopAiming() => playerAnimator.SetBool("Aim", false);

        private void EnteredCrouch() => playerAnimator.SetBool("Crouch", true);

        private void ExitedCrouch() => playerAnimator.SetBool("Crouch", false);


        private void Throw()
        {
            playerAnimator.SetBool("Aim", false);
            playerAnimator.SetBool("Throw", true);
            hasDecoy = false;
            playerAnimator.ResetTrigger("TryPickupDecoy");
        }

        private void OnTrapped()
        {
            playerAnimator.SetTrigger("Trapped");
            playerAnimator.ResetTrigger("Detached");
        }

        private void OnDetached()
        {
            playerAnimator.SetTrigger("Detached");
            playerAnimator.ResetTrigger("Trapped");
        }

        private void OnPickedUpDecoy(GameObject pickUpDecoy)
        {
            if (pickUpDecoy.GetComponent<PickupDecoyGrenade>() == null || hasDecoy) return;
            playerAnimator.SetTrigger("TryPickupDecoy");
            hasDecoy = true;
            currentPickedUpDecoy = pickUpDecoy;
        }
    }
}