//Main author: Andreas Berzelius

using UnityEngine;

namespace PlayerController.PlayerStateMachine
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