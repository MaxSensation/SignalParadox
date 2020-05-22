//Main author: Maximiliam Rosén

using UnityEngine;

namespace Player.PlayerStateMachine
{
    public abstract class PlayerBaseState : State
    {
        private PlayerController _player;
        protected PlayerController Player => _player = _player ? _player : (PlayerController)owner;
        protected float GetGroundCheckDistance => Player.GetGroundCheckDistance();
        protected float GetSkinWidth => Player.GetSkinWidth();
        protected Vector3 Velocity { get => Player.Velocity; set => Player.Velocity = value; }
        protected bool Ischarged { get => Player.IsPlayerCharged; }
    }
}