//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;

namespace Interactables.Triggers.EntitiesTrigger
{
    public class EnemyTrigger : MonoBehaviour
    {
        [SerializeField] private string enemyTag;
        public bool IsTouchingTaggedObject { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(enemyTag))
                IsTouchingTaggedObject = true;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(enemyTag))
                IsTouchingTaggedObject = false;
        }
    }
}
