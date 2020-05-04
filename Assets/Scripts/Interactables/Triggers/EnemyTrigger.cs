//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;

namespace Interactables.Triggers
{
    public class EnemyTrigger : MonoBehaviour
    {
        [SerializeField] private string[] tags;
        public bool IsTouchingTaggedObject { get; private set; }
        public bool IsTouchingLayerObject { get; private set; }
        public string[] TouchingTags { get; private set; }

        private void Awake()
        {
            TouchingTags = new string[tags.Length];
            for (var i = 0; i < tags.Length; i++)
            {
                TouchingTags[i] = "false";
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Colliders")))
            {
                IsTouchingLayerObject = true;
            }
            for (var i = 0; i < tags.Length; i++)
            {
                if (other.CompareTag(tags[i]))
                {
                    IsTouchingTaggedObject = true;
                    TouchingTags[i] = "true";
                }
            }
        }

    
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Colliders")))
            {
                IsTouchingLayerObject = false;
            }
            for (var i = 0; i < tags.Length; i++)
            {
                if (other.CompareTag(tags[i]))
                {
                    IsTouchingTaggedObject = false;
                    TouchingTags[i] = "false";
                }
            }
        }
    }
}
