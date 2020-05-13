//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;

namespace PlayerController.PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/LandState")]
    public class LandState : PlayerBaseState
    {
        [SerializeField] private float soundStrength;
        public override void Enter()
        {
            Player._transmitter.SetSoundStrength(1 - soundStrength);
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