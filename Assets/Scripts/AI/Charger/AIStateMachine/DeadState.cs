//Main author: Andreas Berzelius
//Secondary author: Maximiliam Rosén

using UnityEngine;

namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/DeadState")]
    public class DeadState : ChargerBaseState
    {
        [SerializeField] private AudioClip deathSound;
        private bool isFreezed;

        public override void Enter()
        {
            Ai.agent.enabled = false;
            Ai.AudioSource.PlayOneShot(deathSound);
            base.Enter();
        }

        public override void Run()
        {
            base.Run();
            if (isFreezed) return;
            Ai.aiRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            isFreezed = true;
            Ai.AiCollider.enabled = false;
            Ai.EnemyTrigger.gameObject.SetActive(false);

        }
    }
}