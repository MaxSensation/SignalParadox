using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class AIController : MonoBehaviour
    {
        public State[] states;
        public Transform[] waypoints;
        public LayerMask visionMask;
        public float moveSpeed;
        private StateMachine _stateMachine;
        private bool _stunned;
        internal GameObject target;
        internal NavMeshAgent agent;
        internal Rigidbody rigidbody;


        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            _stateMachine = new StateMachine(this, states);
            target = GameObject.FindWithTag("Player");
            agent = GetComponent<NavMeshAgent>();
            moveSpeed = agent.speed;
        }

        private void Update()
        {
            agent.speed = moveSpeed;
            _stateMachine.Run();
        }
        private IEnumerator StunTime()
        {
            yield return new WaitForSeconds(3);
            _stunned = false;
        }

        internal void ActivateStun()
        {
            _stunned = true;
            StartCoroutine("StunTime");
        }

        public bool IsStunned()
        {
            return _stunned;
        }
    }
}
