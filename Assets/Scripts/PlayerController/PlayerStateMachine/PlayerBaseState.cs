using UnityEngine;

namespace PlayerStateMachine
{
    public abstract class PlayerBaseState : State
    {
        private PlayerController _player;
        protected PlayerController Player => _player = _player ? _player : (PlayerController)owner;
        protected Vector3 Velocity { get => Player.GetVelocity(); set => Player.SetVelocity(value); }
        protected float GetGroundCheckDistance => Player.GetGroundCheckDistance();
        protected float GetSkinWidth => Player.GetSkinWidth();
    }
}