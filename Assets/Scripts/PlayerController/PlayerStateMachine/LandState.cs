using System;
using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/LandState")]
    public class LandState : PlayerBaseState
    {
        public override void Enter()
        {
            Player._transmitter.SetSoundStrength(0.0f);
            if (Player.GetIsPlayerCharged())
                stateMachine.TransitionTo<ChargedState>();
            //Debug.Log("Entered Land State");
            if ( Velocity.magnitude <= 0f || Vector3.Dot(Velocity, Vector3.down) > 0.5f)
                stateMachine.TransitionTo<StandState>();
            else
            {
                // Walk
                stateMachine.TransitionTo<WalkState>();
            }
        }
    }
}