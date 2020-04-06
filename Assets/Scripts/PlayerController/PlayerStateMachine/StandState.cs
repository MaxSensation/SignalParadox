using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/StandState")]
    public class StandState : PlayerBaseState
    {
        public override void Enter()
        {
            Debug.Log("Entered Stand State");
        }
        public override void Run()
        {
            // If any move Input then change to MoveState
            if (new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).magnitude > 0f)
                stateMachine.TransitionTo<WalkState>();
            
            // Enter PushState if E is pressed and interactive box is in range 
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Player.HasPushableBox())
                {
                    stateMachine.TransitionTo<PushingState>();    
                }
            }
            
            // Melee attack
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                stateMachine.StackState<StandState>();
                stateMachine.TransitionTo<MeleeState>();
            }
            
            // Melee attack
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                stateMachine.StackState<StandState>();
                stateMachine.TransitionTo<FireState>();
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
            
            // If Jump Key is pressed then change to Jump State
            if (Input.GetKeyDown(KeyCode.Space))
                stateMachine.TransitionTo<JumpState>();
        }
    }
}