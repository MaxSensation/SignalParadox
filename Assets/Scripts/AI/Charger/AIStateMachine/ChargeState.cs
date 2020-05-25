//Main author: Andreas Berzelius
//Secondary author: Maximiliam Rosén

using UnityEngine;

namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/ChargeState")]
    public class ChargeState : ChargerBaseState
    {
        [SerializeField] private float chargeSpeed;
        [SerializeField] private AudioClip hitWallSound;
        [SerializeField] private float wallSoundThreshold = 10f;
        private float previousFrameSpeed;
        
        public override void Run()
        {
            if (previousFrameSpeed - Ai.aiRigidbody.velocity.magnitude > 0f)
                ChargeEnded();
            Charge();
        }

        private void Charge()
        {
            previousFrameSpeed = Ai.aiRigidbody.velocity.magnitude;
            Ai.aiRigidbody.AddForce(Ai.ChargeDirection.normalized * (chargeSpeed * Time.deltaTime));
            if (!Ai.EnemyTrigger.IsTouchingTaggedObject) return;
            Ai.target.transform.parent = Ai.transform;
            Ai.CaughtPlayer();
        }

        private void ChargeEnded()
        {
            if (previousFrameSpeed - Ai.aiRigidbody.velocity.magnitude > wallSoundThreshold)
            {
                Ai.AudioSource.PlayOneShot(hitWallSound);
                Ai.aiRigidbody.velocity = Vector3.zero;
            }
            previousFrameSpeed = 0f;
            if (Ai.target.transform.IsChildOf(Ai.transform))
                Ai.KillPlayer();
            Ai.target.transform.parent = null;
            Ai.agent.enabled = true;
            stateMachine.TransitionTo<StunState>();
        }
    }
}
