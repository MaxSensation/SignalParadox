using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public abstract class AIController : MonoBehaviour
    {
        [SerializeField] private State[] states;
        [SerializeField] internal Transform[] waypoints;
        [SerializeField] internal LayerMask visionMask;
        [SerializeField] private float moveSpeed;
        
        protected StateMachine _stateMachine;
        protected bool _stunned;
        private Renderer aihRenderer;
        internal GameObject target;
        internal NavMeshAgent agent;
        internal Rigidbody rigidbody;
        private CapsuleCollider _collider;
        internal bool isDead;
        internal bool hasChargedUp;

        public static Action onTrappedPlayer;

        public static Action onCrushedPlayer;

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider>();
            aihRenderer = transform.GetChild(0).GetComponent<Renderer>();
            rigidbody = GetComponent<Rigidbody>();
            target = GameObject.FindWithTag("Player");
            agent = GetComponent<NavMeshAgent>();
            moveSpeed = agent.speed;
            _stateMachine = new StateMachine(this, states);
        }

        private void Update()
        {
            agent.speed = moveSpeed;
            _stateMachine.Run();
        }
        
        public bool IsStunned()
        { return _stunned;
        }      

        public virtual void Die(){}

        public Renderer GetRenderer()
        {
            return aihRenderer;
        }

        public CapsuleCollider GetCollider()
        {
            return _collider;
        }

        internal void TouchingPlayer()
        {
            Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized, out var hit, 1f);
            if (hit.collider && hit.collider.CompareTag("Player"))
                onTrappedPlayer?.Invoke();
        }
    }
}
