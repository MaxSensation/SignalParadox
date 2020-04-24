using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] private string[] tags;
    public bool IsTouchingObject { get; private set; }
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
        for (var i = 0; i < tags.Length; i++)
        {
            if (other.CompareTag(tags[i]))
            {
                IsTouchingObject = true;
                TouchingTags[i] = "true";
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        for (var i = 0; i < tags.Length; i++)
        {
            if (other.CompareTag(tags[i]))
            {
                IsTouchingObject = false;
                TouchingTags[i] = "false";
            }
        }
    }
}
