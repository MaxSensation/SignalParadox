using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/FireState")]
    public class FireState : PlayerBaseState
    {
        public override void Enter()
        {
            Debug.Log("Entered Fire State");
            stateMachine.UnStackState();
        }
    }
}