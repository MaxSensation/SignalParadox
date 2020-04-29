using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/ChargedState")]
    public class ChargedState : PlayerBaseState
    {
        public override void Enter()
        {
            Debug.Log("Enterd Charged State");
        }

    }
}