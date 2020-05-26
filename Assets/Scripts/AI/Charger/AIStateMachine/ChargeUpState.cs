//Main author: Andreas Berzelius
//Secondary author: Maximiliam Rosén

using System;
using UnityEngine;

namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/ChargeUpState")]
    public class ChargeUpState : ChargerBaseState
    {
        public static Action<GameObject> onChargeUpEvent;

        public override void Enter()
        {
            base.Enter();
            Ai.ChargeUp();
            onChargeUpEvent?.Invoke(Ai.gameObject);
            Ai.agent.enabled = false;
        }
    }
}
