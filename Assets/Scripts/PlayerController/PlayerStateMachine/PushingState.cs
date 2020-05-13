//Main author: Maximiliam Rosén

using System;
using UnityEngine;

namespace PlayerController.PlayerStateMachine
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
            Player._transmitter.SetSoundStrength(1 - soundStrength);
            Velocity = Vector3.zero;
            _pushableTransform = Player.currentPushableObject.GetPushableTransform();
            Player._turnWithCamera.enabled = false;
            Player.transform.parent =  _pushableTransform;
        }

        public override void Run()
        {
            if (!Player.endingPushingState)
            {
                base.Run();
                CorrectRotation();
                CorrectPosition();
                if (Player.currentDirection.z > 0)
                {
                    OnPushingStateEvent?.Invoke(true);
                    Player.currentPushableObject.Pushing();
                }
                else
                {
                    Player.currentPushableObject.NotPushing();
                    OnPushingStateEvent?.Invoke(false);
                }
            }
            else{
                OnPushingStateEvent?.Invoke(false);
                OnExitPushingStateEvent?.Invoke();
                Player.EndingPushingState();
            }
        }

        private void CorrectPosition()
        {
            var transform = Player.transform;
            var position = transform.position;
            Player.transform.position = Vector3.Lerp(position, Player.currentPushableObject.GetPushLocation(position), Time.deltaTime * 5f);
        }

        private void CorrectRotation()
        {
            Player._playerMesh.transform.rotation = Quaternion.Lerp(Player._playerMesh.transform.rotation, Quaternion.LookRotation(_pushableTransform.position - Player.transform.position, Vector3.up), Time.deltaTime * 5f);
        }

        public override void Exit()
        {
            base.Exit();
            Player.currentPushableObject.NotPushing();
            Player._turnWithCamera.enabled = true;
            Player.transform.parent =  null;
            Player.currentPushableObject = null;
            Player.endingPushingState = false;
            OnPushingStateEvent?.Invoke(false);
            OnExitPushingStateEvent?.Invoke();
        }
    }
}