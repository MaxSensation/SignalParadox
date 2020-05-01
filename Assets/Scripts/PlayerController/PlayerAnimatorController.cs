using System;
using PlayerStateMachine;
using UnityEngine;
using Traps;

namespace PlayerController
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        private Animator _animator;
        private bool _isCrouching;
        private bool _isAiming;
        private bool _isThrowing;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            CrouchState.onEnteredCrouchEvent += EnteredCrouch;
            CrouchState.onExitCrouchEvent += ExitedCrouch;
            SteamController.onSteamDeath += GasDeath;
            ThrowDecoyGrenade.OnAimingEvent += Aiming;
            ThrowDecoyGrenade.OnThrowEvent += Throw;
            ThrowDecoyGrenade.OnOutOfRangeEvent += StopAiming;
        }



        private void OnDestroy()
        {
            CrouchState.onEnteredCrouchEvent -= EnteredCrouch;
            CrouchState.onExitCrouchEvent -= ExitedCrouch;
            SteamController.onSteamDeath -= GasDeath;
            ThrowDecoyGrenade.OnAimingEvent -= Aiming;
            ThrowDecoyGrenade.OnThrowEvent -= Throw;
            ThrowDecoyGrenade.OnOutOfRangeEvent -= StopAiming;
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

        //Här är test metoderna för de olika death anims
        private void LaserDead()
        {
           
            _animator.SetTrigger("LaserDeath");
        }

        private void GasDeath(GameObject go)
        {
            if(go == gameObject)
            {
                _animator.SetTrigger("GasDeath");
            }   
        }

        private void EnteredCrouch()
        {
            _isCrouching = true;
        }

        private void ExitedCrouch()
        {
            _isCrouching = false;
        }

        private void Update()
        {
            _animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
            _animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
            _animator.SetBool("Crouch", _isCrouching);
            _animator.SetBool("Aim", _isAiming);
            _animator.SetBool("Throw", _isThrowing);           
        }
    }
}
