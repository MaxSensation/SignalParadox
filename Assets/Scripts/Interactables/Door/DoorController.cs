using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Door
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private bool isOpen;
        private BoxCollider _collider;
        private NavMeshObstacle _meshObstacle;
        private MeshRenderer _renderer;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _meshObstacle = GetComponent<NavMeshObstacle>();
            _renderer = GetComponent<MeshRenderer>();        
            if (isOpen)
            {
                OpenDoor();
            }
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
            _collider.enabled = false;
            _meshObstacle.enabled = false;
            _renderer.enabled = false;
        }
    
        private void CloseDoor()
        {
            isOpen = false;
            _meshObstacle.enabled = true;
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
