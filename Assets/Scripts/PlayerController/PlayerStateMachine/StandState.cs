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