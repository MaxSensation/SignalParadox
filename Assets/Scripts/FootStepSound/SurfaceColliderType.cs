//Main author: Ferreira Dos Santos Keziah
using System;
using UnityEngine;
namespace FootStepSound
{
    public class SurfaceColliderType : MonoBehaviour
    {
        [SerializeField] private SurfaceTypes surfaceType;
        public enum SurfaceTypes { Metal, Stairs, Glass}
        public static Action<SurfaceTypes> onEnteredSurfaceZoneEvent;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                onEnteredSurfaceZoneEvent?.Invoke(surfaceType);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                onEnteredSurfaceZoneEvent?.Invoke(SurfaceTypes.Metal);
        }

    }
}
