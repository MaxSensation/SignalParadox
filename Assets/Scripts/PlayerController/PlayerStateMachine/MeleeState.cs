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
            //var hit = Player.GetRayCast(PlayerCameraDirection, hitDistance);
            RaycastHit hit = Player.GetCameraRayCast();

            if (hit.collider && hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Found Enemy");
                hit.collider.GetComponent<AIController>().Die();
            }
        }
    }
}