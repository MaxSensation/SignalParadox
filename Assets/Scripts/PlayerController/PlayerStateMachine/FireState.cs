using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/FireState")]
    public class FireState : PlayerBaseState
    {
        [SerializeField] private GameObject stunGunPatron;
        public override void Enter()
        {
            Debug.Log("Entered Fire State");
            Fire();
            stateMachine.UnStackState();
        }

        private void Fire()
        {
            Instantiate(stunGunPatron, Player.transform.position, Rotation);
        }
    }
}