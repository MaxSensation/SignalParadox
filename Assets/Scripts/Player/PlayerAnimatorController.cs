//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using DecoyGrenade;
using Interactables.Triggers;
using Player.PlayerStateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        [SerializeField] private float smoothTime;
        private Animator animator;
        private bool isCrouching;
        private bool isAiming;
        private bool isThrowing;
        private bool hasDecoy;
        private GameObject currentPickedUpDecoy;
        private Vector2 movement;
        private Vector2 newMovement;

        public static Action OnDeathAnimEnd, OnDeathAnimBeginning;
        public static Action<GameObject> onTouchedGrenade;
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            CrouchState.onEnteredCrouchEvent += EnteredCrouch;
            CrouchState.onExitCrouchEvent += ExitedCrouch;
            ThrowDecoyGrenade.onAimingEvent += Aiming;
            ThrowDecoyGrenade.onThrowEvent += Throw;
            ThrowDecoyGrenade.onAbortAimEvent += StopAiming;
            //PickupDecoyGrenade.onGrenadePickup += OnPickedUpDecoy;
            PlayerInteractionTrigger.onInteractedEvent += OnPickedUpDecoy;

            PushingState.OnEnterPushingStateEvent += HandleEnterPushing;
            PushingState.OnExitPushingStateEvent += HandleExitPushing;
            PushingState.OnPushingStateEvent += HandlePushing;
            HealthSystem.OnPlayerDeath += PlayerDeath;
            PlayerTrapable.onPlayerTrappedEvent += OnTrapped;
            PlayerTrapable.onDetached += OnDetached;
        }

        private void PlayerDeath(HealthSystem.DamageType dT)
        {
            switch (dT)
            {
                case HealthSystem.DamageType.Laser:
                    animator.SetTrigger("LaserDeath");
                    DeathAnimBeginning();
                    break;
                case HealthSystem.DamageType.Steam:
                    animator.SetTrigger("GasDeath");
                    DeathAnimBeginning();
                    break;
                case HealthSystem.DamageType.Bodytrapper:
                    animator.SetTrigger("GasDeath");
                    DeathAnimBeginning();
                    break;
                case HealthSystem.DamageType.Charger:
                    break;
                default:
                    break;
            }
        }

        private void DeathAnimEnd()
        {
            OnDeathAnimEnd?.Invoke();
        }

        private void DeathAnimBeginning()
        {
            OnDeathAnimBeginning?.Invoke();
        }

        private void ActivateDecoyProp()
        {
            onTouchedGrenade?.Invoke(currentPickedUpDecoy);
        }

        private void OnDestroy()
        {
            CrouchState.onEnteredCrouchEvent -= EnteredCrouch;
            CrouchState.onExitCrouchEvent -= ExitedCrouch;
            ThrowDecoyGrenade.onAimingEvent -= Aiming;
            ThrowDecoyGrenade.onThrowEvent -= Throw;
            ThrowDecoyGrenade.onAbortAimEvent -= StopAiming;
            //PickupDecoyGrenade.onGrenadePickup -= OnPickedUpDecoy;
            PlayerInteractionTrigger.onInteractedEvent -= OnPickedUpDecoy;

            PushingState.OnEnterPushingStateEvent -= HandleEnterPushing;
            PushingState.OnExitPushingStateEvent -= HandleExitPushing;
            PushingState.OnPushingStateEvent -= HandlePushing;
            HealthSystem.OnPlayerDeath -= PlayerDeath;
            PlayerTrapable.onPlayerTrappedEvent -= OnTrapped;
            PlayerTrapable.onDetached -= OnDetached;
        }

        private void HandlePushing(bool pushing)
        {
            animator.SetBool("Pushing", pushing);
        }
        private void HandleEnterPushing()
        {
            animator.SetBool("EnteredPushing", true);
        }
        
        private void HandleExitPushing()
        {
            animator.SetBool("EnteredPushing", false);
        }

        private void StopAiming()
        {
            isAiming = false;
        }

        private void Aiming()
        {
            isAiming = true;
            isThrowing = false;
        }

        private void Throw()
        {
            isAiming = false;
            isThrowing = true;
            hasDecoy = false;
            animator.ResetTrigger("TryPickupDecoy");
        }

        private void EnteredCrouch()
        {
            isCrouching = true;
        }

        private void ExitedCrouch()
        {
            isCrouching = false;
        }

        private void OnTrapped()
        {
            animator.SetTrigger("Trapped");
            animator.ResetTrigger("Detached");
        }

        private void OnDetached()
        {
            animator.SetTrigger("Detached");
            animator.ResetTrigger("Trapped");
        }

        public void UpdateMovementInput(InputAction.CallbackContext context)
        {
            newMovement = context.ReadValue<Vector2>();
        }

        private void OnPickedUpDecoy(GameObject pickUpDecoy)
        {
            if (pickUpDecoy.GetComponent<PickupDecoyGrenade>() == null || hasDecoy) return;
            animator.SetTrigger("TryPickupDecoy");
            hasDecoy = true;
            currentPickedUpDecoy = pickUpDecoy;
        }

        private void Update()
        {
            movement = Vector2.Lerp(movement, newMovement, Time.deltaTime * smoothTime);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Horizontal", movement.x);
            animator.SetBool("Crouch", isCrouching);
            animator.SetBool("Aim", isAiming);
            animator.SetBool("Throw", isThrowing);           
        }
    }
}
