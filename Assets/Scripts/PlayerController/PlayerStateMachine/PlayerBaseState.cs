using UnityEngine;

namespace PlayerStateMachine
{
    public abstract class PlayerBaseState : State
    {
        private PlayerController _player;
        protected PlayerController Player => _player = _player ? _player : (PlayerController)owner;
        protected Vector3 Velocity { get => Player.GetVelocity(); set => Player.SetVelocity(value); }
        protected CapsuleCollider PlayerCollider => Player.GetPlayerCollider();
        protected Vector3 Position { get => Player.GetPosition(); set => Player.SetPosition(value); }
        protected Vector3 CameraOffset { get => Player.GetCameraOffset(); set => Player.SetCameraOffset(value); }
        protected Quaternion Rotation { get => Player.GetRotation(); set => Player.SetRotation(value); }
        protected float GetGroundCheckDistance => Player.GetGroundCheckDistance();
        protected float GetSkinWidth => Player.GetSkinWidth();
    }
}