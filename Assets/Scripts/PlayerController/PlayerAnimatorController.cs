using System;
using PlayerStateMachine;
using UnityEngine;

namespace PlayerController
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        private Animator _animator;
        private bool isCrouching;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            CrouchState.onEnteredCrouchEvent += EnteredCrouch;
            CrouchState.onExitCrouchEvent += ExitedCrouch;
        }

        private void EnteredCrouch()
        {
            isCrouching = true;
        }

        private void ExitedCrouch()
        {
            isCrouching = false;
        }

        private void Update()
        {
            _animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
            _animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
            _animator.SetBool("Crouch", isCrouching);
        }
    }
}
