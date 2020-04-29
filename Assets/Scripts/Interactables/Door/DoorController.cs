using System.Linq;
using UnityEngine;

namespace Door
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private bool isOpen;
        private BoxCollider _collider;
        private MeshRenderer _renderer;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _renderer = GetComponent<MeshRenderer>();        
            if (isOpen)
            {
                OpenDoor();
            }
            ButtonController.onButtonPressed += OnButtonPressed;
        }
    
        private void OnDestroy()
        {
            ButtonController.onButtonPressed -= OnButtonPressed;
        }

        private void OpenDoor()
        {
            isOpen = true;
            _collider.enabled = false;
            _renderer.enabled = false;
        }
    
        private void CloseDoor()
        {
            isOpen = false;
            _collider.enabled = true;
            _renderer.enabled = true;
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
