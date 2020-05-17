//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;

namespace Player.PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/StandState")]
    public class StandState : PlayerBaseState
    {
        [SerializeField] private float soundStrength;
        public override void Enter()
        {
            Player._transmitter.SetSoundStrength(1 - soundStrength);
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