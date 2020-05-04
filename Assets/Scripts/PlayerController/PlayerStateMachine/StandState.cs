//Main author: Maximiliam Rosén

using UnityEngine;

namespace PlayerController.PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/StandState")]
    public class StandState : PlayerBaseState
    {
        public override void Enter()
        {
            Player._transmitter.SetSoundStrength(0.99f);
        }
        public override void Run()
        {
            if (Player.GetIsPlayerCharged())
                stateMachine.TransitionTo<ChargedState>();

            // If any move Input then change to MoveState
            if (Player.currentDirection.magnitude > 0f)
                stateMachine.TransitionTo<WalkState>();
            
            // Enter Crouch if Control is pressed 
            if (Player.hasInputCrouch)
            {
                stateMachine.TransitionTo<CrouchState>();
            }
            
            // Check for ground
            var grounded = Player.GetRayCast(Vector3.down, GetGroundCheckDistance + GetSkinWidth).collider;
            
            // If not grounded then change to In Air State
            if (!grounded)
                stateMachine.TransitionTo<InAirState>();
        }
    }
}