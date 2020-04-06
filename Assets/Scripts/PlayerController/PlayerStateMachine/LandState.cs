using System;
using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/LandState")]
    public class LandState : PlayerBaseState
    {
        public override void Enter()
        {
            Debug.Log("Entered Land State");
            if ( Velocity.magnitude <= 0f || Vector3.Dot(Velocity, Vector3.down) > 0.5f)
                stateMachine.TransitionTo<StandState>();
            else
            {
                // Run if shift is pressed
                if (Input.GetKey(KeyCode.LeftShift))
                    stateMachine.TransitionTo<RunState>();
                // Walk
                stateMachine.TransitionTo<WalkState>();
            }
        }
    }
}