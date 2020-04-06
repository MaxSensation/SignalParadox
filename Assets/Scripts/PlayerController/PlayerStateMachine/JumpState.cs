using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/JumpState")]
    public class JumpState : PlayerBaseState
    {
        [SerializeField] private float jumpHeight;
        
        public override void Enter()
        {
            Debug.Log("Entered Jump State");
            // Add JumpForce and GravityForce to PlayerVelocity
            Velocity += Vector3.up * jumpHeight;
            // Change to In Air State
            stateMachine.TransitionTo<InAirState>();
        }
    }
}