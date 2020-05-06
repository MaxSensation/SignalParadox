//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using Interactables.Traps;
using PlayerController.PlayerStateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        [SerializeField] private float smoothTime;
        private Animator _animator;
        private bool _isCrouching;
        private bool _isAiming;
        private bool _isThrowing;
        private Vector2 _movement;
        private Vector2 _newMovement;

        public static Action OnDeathAnimEnd, OnDeathAnimBeginning;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            CrouchState.onEnteredCrouchEvent += EnteredCrouch;
            CrouchState.onExitCrouchEvent += ExitedCrouch;
            ThrowDecoyGrenade.OnAimingEvent += Aiming;
            ThrowDecoyGrenade.OnThrowEvent += Throw;
            ThrowDecoyGrenade.OnOutOfRangeEvent += StopAiming;
            PushingState.OnEnterPushingStateEvent += HandleEnterPushing;
            PushingState.OnExitPushingStateEvent += HandleExitPushing;
            PushingState.OnPushingStateEvent += HandlePushing;
            HealthSystem.OnPlayerDeath += PlayerDeath;
        }

        private void PlayerDeath(HealthSystem.DamageType dT)
        {
            switch (dT)
            {
                case HealthSystem.DamageType.Laser:
                    _animator.SetTrigger("LaserDeath");
                    DeathAnimBeginning();
                    break;
                case HealthSystem.DamageType.Steam:
                    _animator.SetTrigger("GasDeath");
                    DeathAnimBeginning();
                    break;
                case HealthSystem.DamageType.Bodytrapper:
                    _animator.SetTrigger("GasDeath");
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

        private void OnDestroy()
        {
            CrouchState.onEnteredCrouchEvent -= EnteredCrouch;
            CrouchState.onExitCrouchEvent -= ExitedCrouch;
            ThrowDecoyGrenade.OnAimingEvent -= Aiming;
            ThrowDecoyGrenade.OnThrowEvent -= Throw;
            ThrowDecoyGrenade.OnOutOfRangeEvent -= StopAiming;
            PushingState.OnEnterPushingStateEvent -= HandleEnterPushing;
            PushingState.OnExitPushingStateEvent -= HandleExitPushing;
            PushingState.OnPushingStateEvent -= HandlePushing;
            HealthSystem.OnPlayerDeath -= PlayerDeath;
        }

        private void HandlePushing(bool pushing)
        {
            _animator.SetBool("Pushing", pushing);
        }
        private void HandleEnterPushing()
        {
            _animator.SetBool("EnteredPushing", true);
        }
        
        private void HandleExitPushing()
        {
            _animator.SetBool("EnteredPushing", false);
        }

        private void StopAiming()
        {
            _isAiming = false;
        }

        private void Aiming()
        {
            _isAiming = true;
            _isThrowing = false;
        }

        private void Throw()
        {
            _isAiming = false;
            _isThrowing = true;
        }

        private void EnteredCrouch()
        {
            _isCrouching = true;
        }

        private void ExitedCrouch()
        {
            _isCrouching = false;
        }

        public void UpdateMovementInput(InputAction.CallbackContext context)
        {
            _newMovement = context.ReadValue<Vector2>();
        }

        private void Update()
        {
            _movement = Vector2.Lerp(_movement, _newMovement, Time.deltaTime * smoothTime);
            _animator.SetFloat("Vertical", _movement.y);
            _animator.SetFloat("Horizontal", _movement.x);
            _animator.SetBool("Crouch", _isCrouching);
            _animator.SetBool("Aim", _isAiming);
            _animator.SetBool("Throw", _isThrowing);           
        }
    }
}
