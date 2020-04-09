using AI;
using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/MeleeState")]
    public class MeleeState : PlayerBaseState
    {
        [SerializeField] private float hitDistance;

        public override void Enter()
        {
            Melee();
            Debug.Log("Entered Melee");
            stateMachine.UnStackState();
        }

        private void Melee()
        {
            var hit = Player.GetMeeleRayCast(hitDistance);
            if (hit.collider)
                Debug.Log(hit.collider.name);
            if (hit.collider && hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Found Enemy");
                hit.collider.GetComponent<AIController>().Die();
            }
        }
    }
}