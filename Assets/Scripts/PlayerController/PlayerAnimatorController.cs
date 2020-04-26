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
            _animator.SetFloat("vertical", Input.GetAxis("Vertical"));
            _animator.SetFloat("horizontal", Input.GetAxis("Horizontal"));
            if (Input.GetKeyDown(KeyCode.Space))
                _animator.SetTrigger("jump");
            
        }
    }
}
