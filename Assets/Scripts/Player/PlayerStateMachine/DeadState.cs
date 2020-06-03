//Main author: Maximiliam Rosén

using UnityEngine;

namespace Player.PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/DeadState")]
    public class DeadState : PlayerBaseState
    {
        public override void Enter()
        {
            base.Enter();
            TurnWithCamera.Active = false;
            Velocity = Vector3.zero;
        }

        public override void Run()
        {
            base.Run();
            Player.GetComponent<PlayerTrapable>().DetachAllBodyTrappers();
        }
    }
}