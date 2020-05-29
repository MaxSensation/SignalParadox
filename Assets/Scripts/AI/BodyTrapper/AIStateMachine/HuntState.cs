//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/HuntState")]
    public class HuntState : BodyTrapperBaseState
    {
        [SerializeField] private float jumpDistance;
        [SerializeField] private float maxMinLookRange;
        [SerializeField] private AudioClip clawSnapping;
        [SerializeField] private AudioClip defaultSound;

        public static Action<GameObject> onHuntEvent; 

        public override void Enter()
        {
            base.Enter();
            Ai.audioSource.clip = clawSnapping;
            Ai.audioSource.volume = 0.5f;
            Ai.audioSource.Play();
            onHuntEvent?.Invoke(Ai.gameObject);
        }

        public override void Run()
        {
            if (!Ai.IsStunned && Ai.agent.enabled)
                Ai.agent.SetDestination(Ai.target.transform.position);

            if (Ai.isPlayerAlive && Ai.PlayerInSight() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < jumpDistance && Ai.LookingAtPlayer(Ai, maxMinLookRange))
            {
                Ai.audioSource.volume = 0.1f;
                Ai.audioSource.clip = defaultSound;
                stateMachine.TransitionTo<ChargeState>();
            }

            if (Ai.PlayerInSight() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < jumpDistance)
            {
                var directionToPlayer = (Ai.target.transform.position - Ai.transform.position);
                var targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z), Vector3.up);
                Ai.transform.rotation = Quaternion.Lerp(Ai.transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }
}