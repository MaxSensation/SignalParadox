using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] private string enemyTag;
    public bool IsTouchingEnemy { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            IsTouchingEnemy = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            IsTouchingEnemy = false;
        }
    }
}
