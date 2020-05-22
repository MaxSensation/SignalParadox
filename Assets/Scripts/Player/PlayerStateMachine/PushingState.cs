//Main author: Maximiliam Rosén

using System;
using UnityEngine;

namespace Player.PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/PushingState")]
    public class PushingState : PlayerBaseState
    {
        public static Action OnEnterPushingStateEvent;
        public static Action OnExitPushingStateEvent;
        public static Action<Boolean> OnPushingStateEvent;
        [SerializeField] private float soundStrength;
        private Transform _pushableTransform;
        public override void Enter()
        {
            base.Enter();
            OnEnterPushingStateEvent?.Invoke();
            Player.Transmitter.SetSoundStrength(1 - soundStrength);
            Velocity = Vector3.zero;
            _pushableTransform = Player.CurrentPushableObject.GetPushableTransform();
            TurnWithCamera.Active = false;
            Player.transform.parent =  _pushableTransform;
        }

        public override void Run()
        {
            if (!Player.EndingPushingState)
            {
                base.Run();
                CorrectRotation();
                CorrectPosition();
                if (Player.CurrentDirection.z > 0)
                {
                    OnPushingStateEvent?.Invoke(true);
                    Player.CurrentPushableObject.Pushing();
                }
                else
                {
                    Player.CurrentPushableObject.NotPushing();
                    OnPushingStateEvent?.Invoke(false);
                }
            }
            else{
                OnPushingStateEvent?.Invoke(false);
                OnExitPushingStateEvent?.Invoke();
                Player.StartEndingPushingState();
            }
        }

        private void CorrectPosition()
        {
            var transform = Player.transform;
            var position = transform.position;
            Player.transform.position = Vector3.Lerp(position, Player.CurrentPushableObject.GetPushLocation(position), Time.deltaTime * 5f);
        }

        private void CorrectRotation()
        {
            Player.PlayerMesh.transform.rotation = Quaternion.Lerp(Player.PlayerMesh.transform.rotation, Quaternion.LookRotation(_pushableTransform.position - Player.transform.position, Vector3.up), Time.deltaTime * 5f);
        }

        public override void Exit()
        {
            base.Exit();
            Player.CurrentPushableObject.NotPushing();
            TurnWithCamera.Active = true;
            Player.transform.parent =  null;
            Player.CurrentPushableObject = null;
            Player.EndingPushingState = false;
            OnPushingStateEvent?.Invoke(false);
            OnExitPushingStateEvent?.Invoke();
        }
    }
}