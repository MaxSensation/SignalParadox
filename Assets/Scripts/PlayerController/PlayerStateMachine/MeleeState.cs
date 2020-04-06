using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/MeleeState")]
    public class MeleeState : PlayerBaseState
    {
        public override void Enter()
        {
            Debug.Log("Entered Melee State");
            stateMachine.UnStackState();
        }
    }
}