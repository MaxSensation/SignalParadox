﻿using System;
using Interactables.Pushables;
using PlayerStateMachine;
using UnityEngine;
using Traps;
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
        private bool _isPushing;
        private Vector2 _movement;
        private Vector2 _newMovement;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            CrouchState.onEnteredCrouchEvent += EnteredCrouch;
            CrouchState.onExitCrouchEvent += ExitedCrouch;
            SteamController.onSteamDeath += GasDeath;
            LaserController.onLaserDeath += LaserDead;
            ThrowDecoyGrenade.OnAimingEvent += Aiming;
            ThrowDecoyGrenade.OnThrowEvent += Throw;
            ThrowDecoyGrenade.OnOutOfRangeEvent += StopAiming;
            PushingState.OnEnterPushingStateEvent += HandlePushing;
            PushingState.OnPushingStateEvent += HandleEnterPushing;
        }

        private void OnDestroy()
        {
            CrouchState.onEnteredCrouchEvent -= EnteredCrouch;
            CrouchState.onExitCrouchEvent -= ExitedCrouch;
            SteamController.onSteamDeath -= GasDeath;
            ThrowDecoyGrenade.OnAimingEvent -= Aiming;
            ThrowDecoyGrenade.OnThrowEvent -= Throw;
            ThrowDecoyGrenade.OnOutOfRangeEvent -= StopAiming;
            PushingState.OnEnterPushingStateEvent += HandlePushing;
            PushingState.OnPushingStateEvent -= HandleEnterPushing;
        }
        
        private void HandleEnterPushing(bool pushing)
        {
            _animator.SetBool("Pushing", pushing);
        }
        private void HandlePushing()
        {
            _isPushing = !_isPushing;
            _animator.SetBool("EnteredPushing", _isPushing);
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
        private void LaserDead(GameObject go)
        {
            if (go == gameObject)
            {
                _animator.SetTrigger("LaserDeath");
            }  
            
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
