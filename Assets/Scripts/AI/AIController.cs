//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public abstract class AIController : MonoBehaviour
    {
        [SerializeField] private State[] states;
        [SerializeField] internal Transform[] waypoints;
        [SerializeField] internal LayerMask visionMask;

        protected StateMachine stateMachine;
        protected bool isStunned;
        internal GameObject target;
        internal NavMeshAgent agent;
        internal Rigidbody aiRigidbody;
        internal bool isDead;
        internal CapsuleCollider AiCollider { get; private set; }

        protected void Awake()
        {
            AiCollider = GetComponent<CapsuleCollider>();
            aiRigidbody = GetComponent<Rigidbody>();
            target = FindObjectOfType<Player.PlayerController>().gameObject;
            agent = GetComponent<NavMeshAgent>();
            stateMachine = new StateMachine(this, states);
        }

        private void Update()
        {
            stateMachine.Run();
        }

        internal bool IsStunned() { return isStunned; }

        protected virtual void Die() { }

        internal bool LookingAtPlayer(AIController Ai, float maxMinLookRange)
        {
            var transform = Ai.transform;
            var enemyPosition = transform.position;
            enemyPosition = new Vector3(enemyPosition.x, 0, enemyPosition.z);
            var playerPosition = Ai.target.transform.position;
            playerPosition = new Vector3(playerPosition.x, 0, playerPosition.z);
            return Vector3.Dot(transform.forward.normalized, (playerPosition - enemyPosition).normalized) > maxMinLookRange;
        }
    }
}
