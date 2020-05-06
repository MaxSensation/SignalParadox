//Main author: Maximiliam Rosén

using UnityEngine;

namespace PlayerController.PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/DeadState")]
    public class DeadState : PlayerBaseState
    {
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Entered DeadState");
            Player._turnWithCamera.enabled = false;
            Velocity = Vector3.zero;
        }

        public override void Run()
        {
            base.Run();
            Player.GetComponent<PlayerTrapable>().DetachAllTrappers();
        }
    }
}