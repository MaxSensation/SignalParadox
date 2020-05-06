//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System.Linq;
using Interactables.Button;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables.Door
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private bool isOpen;
        [SerializeField] private bool isAutoClosing;
        private Animator _animator;
        public UnityEvent Open;
        public UnityEvent Close;
        private AutoCloseTrigger _autoCloseTrigger;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
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

        private void OpenDoor()
        {
            isOpen = true;
            Open?.Invoke();
            _animator.SetBool("IsOpen", isOpen);
        }
    
        private void CloseDoor()
        {
            isOpen = false;
            Close?.Invoke();
            _animator.SetBool("IsOpen", isOpen);
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
