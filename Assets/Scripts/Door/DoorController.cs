//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System.Linq;
using Interactables.Button;
using Interactables.Triggers.Platform;
using UnityEngine;

namespace Door
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] [Tooltip("Is the door open at start.")] private bool isOpen;
        [SerializeField] [Tooltip("Is the door auto closing after the player have passed thru the door.")] private bool isAutoClosing;
        [SerializeField] [Tooltip("The size of the player interrupt size.")] private Vector3 playerCheckSize;
        [SerializeField] [Tooltip("The position of the player interruptBox.")] private Vector3 playerCheckPosition;
        [SerializeField] [Tooltip("Opening and closing AudioClips")] private AudioClip openSound, closeSound;
        private AutoCloseTrigger autoCloseTrigger;
        private AudioSource audioSource;
        private MeshRenderer meshRenderer;
        private Animator doorAnimator;
        private bool isClosing;

        private void Awake()
        {
            doorAnimator = GetComponent<Animator>();
            meshRenderer = GetComponent<MeshRenderer>();
            audioSource = GetComponent<AudioSource>();
            if (isOpen) OpenDoor();
            if (isAutoClosing)
            {
                autoCloseTrigger = transform.GetComponentInChildren<AutoCloseTrigger>();
                autoCloseTrigger.onAutoCloseEvent += CloseDoor;
            }
            ButtonController.onButtonPressedEvent += OnButtonPressed;
            PlatformTrigger.onButtonPressedEvent += OnButtonPressed;
        }

        private void OnDestroy()
        {
            ButtonController.onButtonPressedEvent -= OnButtonPressed;
            PlatformTrigger.onButtonPressedEvent -= OnButtonPressed;
            if (isAutoClosing) autoCloseTrigger.onAutoCloseEvent -= CloseDoor;
        }

        private bool CheckForPlayer()
        {
            return Physics.OverlapBox(
                meshRenderer.bounds.center + playerCheckPosition,
                playerCheckSize / 2, transform.rotation,
                LayerMask.GetMask("Player")
                ).Length > 0;
        }

        private void Update()
        {
            if (!isClosing) return;
            if (CheckForPlayer()) AbortClose();
        }

        private void AbortClose()
        {
            if (isAutoClosing)
                autoCloseTrigger.SetHasClosed(false);
            doorAnimator.SetFloat("SpeedModifier", -1f);
            isOpen = true;
            doorAnimator.SetBool("IsOpen", isOpen);
        }

        [ContextMenu("Open")]
        private void OpenDoor()
        {
            if (isAutoClosing)
                autoCloseTrigger.SetHasClosed(false);
            isOpen = true;
            doorAnimator.SetBool("IsOpen", isOpen);
            audioSource.PlayOneShot(openSound);
        }

        [ContextMenu("Close")]
        private void CloseDoor()
        {
            if (!isOpen) return;
            isOpen = false;
            doorAnimator.Play("DoorOpen", 0, 1f);
            doorAnimator.SetFloat("SpeedModifier", 1f);
            doorAnimator.SetBool("IsOpen", isOpen);
            isClosing = true;
            audioSource.PlayOneShot(closeSound);
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
