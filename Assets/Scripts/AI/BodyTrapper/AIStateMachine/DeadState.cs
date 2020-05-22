//Main author: Maximiliam Rosén

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