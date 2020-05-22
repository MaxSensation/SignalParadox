//Main author: Maximiliam Rosén

using UnityEngine;

namespace Player.PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/DeadState")]
    public class DeadState : PlayerBaseState
    {
        [SerializeField] private float soundStrength;
        public override void Enter()
        {
            base.Enter();
            Player.Transmitter.SetSoundStrength(1 - soundStrength);
            Debug.Log("Entered DeadState");
            TurnWithCamera.Active = false;
            Velocity = Vector3.zero;
        }

        public override void Run()
        {
            base.Run();
            Player.GetComponent<PlayerTrapable>().DetachAllTrappers();
        }
    }
}