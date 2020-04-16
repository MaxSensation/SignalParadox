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
        }

        private void Fire()
        {
            if (Player.hasReloaded)
            {
                Instantiate(stunGunPatron, Player.transform.position, Quaternion.Euler(CameraRotation.y - 10f, CameraRotation.x, Rotation.z));
                Player.hasReloaded = false;
                stateMachine.TransitionTo<ReloadState>();
            }
            else
                stateMachine.UnStackState();
        }
    }
}