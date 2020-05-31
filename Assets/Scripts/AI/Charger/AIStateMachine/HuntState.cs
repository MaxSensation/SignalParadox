//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;

namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/HuntState")]
    public class HuntState : ChargerBaseState
    {
        [SerializeField] private float chargeDistance;
        [SerializeField] private float searchingRange;
        [SerializeField] private float maxLookAngle;
        [SerializeField] private float minturnSpeed = 10;
        [SerializeField] private float maxturnSpeed = 45;
        private float currentTurnSpeed;

        public override void Run()
        {
            if (Ai.PlayerInSight())
            {
                Ai.agent.SetDestination(Ai.target.transform.position);
                LookAtPlayer();
                if (!Ai.LookingAtPlayer(Ai, maxLookAngle) || !CanCharge()) return;
                stateMachine.TransitionTo<ChargeUpState>();
                Ai.SetChargeDirection();
            }
            else
            {
                if (Vector3.Distance(Ai.transform.position, Ai.target.transform.position) > searchingRange)
                    stateMachine.TransitionTo<PatrolState>();   
                else
                    Ai.agent.SetDestination(Ai.target.transform.position);
            }
        }

        private void LookAtPlayer()
        {
            var newRotation = Quaternion.LookRotation(Ai.target.transform.position - Ai.transform.position, Vector3.up);
            newRotation.x = 0.0f;
            newRotation.z = 0.0f;
            if ((Ai.target.transform.position - Ai.transform.position).magnitude < Ai.AiCollider.radius * 3.5)
                currentTurnSpeed = maxturnSpeed;
            else
                currentTurnSpeed = minturnSpeed;
            Ai.transform.rotation = Quaternion.Slerp(Ai.transform.rotation, newRotation, Time.deltaTime * currentTurnSpeed);
        }

        private bool CanCharge()
        {
            if (Vector3.Distance(Ai.transform.position, Ai.target.transform.position) >= chargeDistance) return false;
            var chargerTransform = Ai.transform;
            var capsulePosition = chargerTransform.position + Ai.AiCollider.center;
            var distanceToPoints = (Ai.AiCollider.height / 2) - Ai.AiCollider.radius;
            var point1 = capsulePosition + Vector3.up * distanceToPoints;
            var point2 = capsulePosition + Vector3.down * distanceToPoints;
            Physics.CapsuleCast(point1, point2, AiCollider.radius, chargerTransform.forward.normalized, out var hit, (Ai.target.transform.position - chargerTransform.position).magnitude, Ai.visionMask);
            //if very close to player canCharge is true
            if ((Ai.target.transform.position - Ai.transform.position).magnitude < Ai.AiCollider.radius * 3.5)
                return true;
            return !(hit.collider);
        }
    }
}