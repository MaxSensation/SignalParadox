using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/StandState")]
    public class StandState : PlayerBaseState
    {
        public override void Enter()
        {
            PlayerController.PlayerController.onSoundLevelChanged?.Invoke(1f);
        }
        public override void Run()
        {
            if (Player.GetIsPlayerCharged())
                stateMachine.TransitionTo<ChargedState>();

            // If any move Input then change to MoveState
            if (new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).magnitude > 0f)
                stateMachine.TransitionTo<WalkState>();
            
            // Enter PushState if E is pressed and interactive box is in range 
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Player.CheckSimpleShortRayCast("PushableBox"))
                {
                    stateMachine.TransitionTo<PushingState>();    
                }
            }

            // Enter Crouch if Control is pressed 
            if (Input.GetKeyDown(KeyCode.LeftControl))
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