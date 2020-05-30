//Main author: Maximiliam Rosén

using System;
using UnityEngine;

namespace Player.PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/PushingState")]
    public class PushingState : PlayerBaseState
    {
        [SerializeField] private float soundStrength;
        [SerializeField] private float smoothness = 5f;
        private Transform pushableTransform;
        public static Action onEnterPushingStateEvent, onExitPushingStateEvent;
        public static Action<bool> onPushingStateEvent;
        
        public override void Enter()
        {
            base.Enter();
            onEnterPushingStateEvent?.Invoke();
            Player.Transmitter.SetSoundStrength(soundStrength);
            Velocity = Vector3.zero;
            pushableTransform = Player.CurrentPushableObject.GetPushableTransform();
            TurnWithCamera.Active = false;
            Player.transform.parent =  pushableTransform;
        }

        public override void Run()
        {
            base.Run();
            if (!Player.EndingPushingState)
            {
                CorrectRotation();
                CorrectPosition();
                if (Player.CurrentDirection.z > 0)
                {
                    onPushingStateEvent?.Invoke(true);
                    Player.CurrentPushableObject.Pushing();
                }
                else
                {
                    Player.CurrentPushableObject.NotPushing();
                    onPushingStateEvent?.Invoke(false);
                }
            }
            else{
                onPushingStateEvent?.Invoke(false);
                onExitPushingStateEvent?.Invoke();
                Player.StartEndingPushingState();
            }
        }

        private void CorrectPosition()
        {
            var transform = Player.transform;
            var position = transform.position;
            Player.transform.position = 
                Vector3.Lerp(position, Player.CurrentPushableObject.GetPushLocation(position), 
                    Time.deltaTime * smoothness);
        }

        private void CorrectRotation()
        {
            Player.PlayerMesh.transform.rotation = 
                Quaternion.Lerp(Player.PlayerMesh.transform.rotation, 
                    Quaternion.LookRotation(pushableTransform.position - Player.transform.position, Vector3.up), 
                    Time.deltaTime * smoothness);
        }

        public override void Exit()
        {
            base.Exit();
            Player.CurrentPushableObject.NotPushing();
            TurnWithCamera.Active = true;
            Player.transform.parent =  null;
            Player.CurrentPushableObject = null;
            Player.EndingPushingState = false;
            onPushingStateEvent?.Invoke(false);
            onExitPushingStateEvent?.Invoke();
        }
    }
}