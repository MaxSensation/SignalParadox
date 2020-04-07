using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        public State[] states;
        public LayerMask visionMask;
        public float moveSpeed;
        private StateMachine _stateMachine;
        internal GameObject target;
        internal NavMeshAgent agent;

        private void Awake()
        {
            _stateMachine = new StateMachine(this, states);
            target = GameObject.FindWithTag("Player");
            agent = GetComponent<NavMeshAgent>();
            moveSpeed = agent.speed;
        }

        private void Update()
        {
            agent.speed = moveSpeed;
            _stateMachine.Run();
            agent.SetDestination(target.transform.position);
        }
    }
}
