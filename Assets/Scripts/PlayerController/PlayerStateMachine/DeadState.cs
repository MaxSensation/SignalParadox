//Main author: Maximiliam Rosén

using UnityEngine;

namespace PlayerController.PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/DeadState")]
    public class DeadState : PlayerBaseState
    {
        public override void Enter()
        {
            base.Enter();
        }
    }
}