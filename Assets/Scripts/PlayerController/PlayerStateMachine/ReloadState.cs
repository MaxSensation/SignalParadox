using System.Collections;
using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/ReloadState")]
    public class ReloadState : PlayerBaseState
    {
        public override void Enter()
        {
            Debug.Log("Entered Reload State");
            if (Player.GetIsPlayerCharged())
                stateMachine.TransitionTo<ChargedState>();
            Player.Reloading();
            stateMachine.UnStackState();
        }
    }
}