﻿//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System.Linq;
using Interactables.Button;
using UnityEngine;

namespace Interactables.Door
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField][Tooltip("Is the door open at start.")] private bool isOpen;
        [SerializeField][Tooltip("Is the door auto closing after the player have passed thru the door.")] private bool isAutoClosing;
        [SerializeField][Tooltip("The size of the player interrupt size.")] private Vector3 playerCheckSize;
        [SerializeField][Tooltip("The position of the player interruptBox.")] private Vector3 playerCheckPosition;
        [SerializeField] private AudioClip openSound;
        [SerializeField] private AudioClip closeSound;
        private AutoCloseTrigger _autoCloseTrigger;
        private AudioSource _audioSource;
        private MeshRenderer _renderer;
        private Animator _animator;
        private bool _isClosing;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<MeshRenderer>();
            _audioSource = GetComponent<AudioSource>();
            if (isOpen)
                OpenDoor();
            if (isAutoClosing)
            {
                _autoCloseTrigger = transform.parent.Find("AutoCloseTrigger").GetComponent<AutoCloseTrigger>();
                _autoCloseTrigger.OnAutoClose += CloseDoor;
            }
            ButtonController.onButtonPressed += OnButtonPressed;
            PlatformTrigger.onButtonPressed += OnButtonPressed;

        }
    
        private void OnDestroy()
        {
            ButtonController.onButtonPressed -= OnButtonPressed;
            PlatformTrigger.onButtonPressed -= OnButtonPressed;
            if(isAutoClosing)
                _autoCloseTrigger.OnAutoClose -= CloseDoor;
        }

        private bool CheckForPlayer()
        {
            return Physics.OverlapBox(_renderer.bounds.center + playerCheckPosition, playerCheckSize/2, transform.rotation, LayerMask.GetMask("Player")).Length > 0;
        }

        private void Update()
        {
            if (!_isClosing) return;
            if (CheckForPlayer()) AbortClose();
        }

        private void AbortClose()
        {
            _autoCloseTrigger.SetHasClosed(false);
            _animator.SetFloat("SpeedModifier", -1f);
            isOpen = true;
            _animator.SetBool("IsOpen", isOpen);
        }

        public void OpenDoor()
        {
            isOpen = true;
            _animator.SetBool("IsOpen", isOpen);
            _audioSource.PlayOneShot(openSound);
        }
    
        public void CloseDoor()
        {
            isOpen = false;
            _animator.Play("DoorOpen", 0, 1f);
            _animator.SetFloat("SpeedModifier", 1f);
            _animator.SetBool("IsOpen", isOpen);
            _isClosing = true;
            _audioSource.PlayOneShot(closeSound);
        }

        private void OnButtonPressed(GameObject[] interactableObjects)
        {
            if (!interactableObjects.Contains(gameObject)) return;
            if (isOpen)
                CloseDoor();
            else
                OpenDoor();
        }
    }
}
