//Main author: Maximiliam Rosén

using UnityEngine;

namespace Player.PlayerStateMachine
{
    public abstract class PlayerBaseState : State
    {
        private PlayerController player;
        protected PlayerController Player => player = player ? player : (PlayerController)owner;
        protected float GetGroundCheckDistance => Player.GetGroundCheckDistance();
        protected float GetSkinWidth => Player.GetSkinWidth();
        protected Vector3 Velocity { get => Player.Velocity; set => Player.Velocity = value; }
        protected bool IsCharged => Player.IsPlayerCharged;
    }
}