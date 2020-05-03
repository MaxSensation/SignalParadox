using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/PushingState")]
    public class PushingState : PlayerBaseState
    {
        public override void Enter()
        {
            Player._transmitter.SetSoundStrength(0.2f);
        }
    }
}