//Main author: Maximiliam Rosén

using AI.AIStateMachine;
using AI.Charger.AIStateMachine;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/DeadState")]
    public class DeadState : BodyTrapperBaseState
    {
        [SerializeField] private AudioClip deathSound;

        public override void Enter()
        {
            Ai.agent.enabled = false;
            Ai.audioSource.PlayOneShot(deathSound);
            base.Enter();
        }
    }
}