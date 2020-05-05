//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System.Linq;
using Interactables.Button;
using UnityEngine;

namespace Interactables.Door
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private bool isOpen;
        private Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            if (isOpen)
                OpenDoor();
            ButtonController.onButtonPressed += OnButtonPressed;
            PlatformTrigger.onButtonPressed += OnButtonPressed;
        }
    
        private void OnDestroy()
        {
            ButtonController.onButtonPressed -= OnButtonPressed;
            PlatformTrigger.onButtonPressed -= OnButtonPressed;
        }

        private void OpenDoor()
        {
            isOpen = true;
            _animator.SetBool("IsOpen", isOpen);
        }
    
        private void CloseDoor()
        {
            isOpen = false;
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
