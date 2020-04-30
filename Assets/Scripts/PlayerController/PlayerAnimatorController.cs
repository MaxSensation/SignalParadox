using System;
using PlayerStateMachine;
using UnityEngine;
using Traps;

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
            SteamController.onSteamDeath += GasDeath;
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
        //

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
