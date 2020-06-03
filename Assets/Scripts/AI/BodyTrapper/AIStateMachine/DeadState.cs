//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/DeadState")]
    public class DeadState : BodyTrapperBaseState
    {
        [SerializeField] private AudioClip deathSound;
        private bool isFreezed;

        public override void Enter()
        {
            Ai.agent.enabled = false;
            Ai.audioSource.PlayOneShot(deathSound);
            base.Enter();
        }

        public override void Run()
        {
            base.Run();
            if (!Ai.Grounded() || isFreezed) return;
            Ai.aiRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            isFreezed = true;
            Ai.AiCollider.enabled = false;
        }
    }
}