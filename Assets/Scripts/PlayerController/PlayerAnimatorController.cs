using System;
using UnityEngine;

namespace PlayerController
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
            _animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
            if (Input.GetKeyDown(KeyCode.Space))
                _animator.SetTrigger("Jump");
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                _animator.SetBool("Crouch", true);
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                _animator.SetBool("Crouch", false);
            }


        }
    }
}
