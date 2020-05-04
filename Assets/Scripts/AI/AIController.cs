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

        protected StateMachine _stateMachine;
        protected bool _stunned;
        private Renderer aihRenderer;
        internal GameObject target;
        internal NavMeshAgent agent;
        internal Rigidbody rigidbody;
        internal CapsuleCollider _collider;
        internal bool isDead;

        protected void Awake()
        {
            _collider = GetComponent<CapsuleCollider>();
            aihRenderer = transform.GetChild(0).GetComponent<Renderer>();
            rigidbody = GetComponent<Rigidbody>();
            target = GameObject.FindWithTag("Player");
            agent = GetComponent<NavMeshAgent>();
            _stateMachine = new StateMachine(this, states);
        }

        private void Update()
        {
            _stateMachine.Run();
        }

        protected internal bool IsStunned()
        {
            return _stunned;
        }

        protected internal virtual void Die() { }

        protected internal Renderer GetRenderer()
        {
            return aihRenderer;
        }

        protected internal CapsuleCollider GetCollider()
        {
            return _collider;
        }

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
