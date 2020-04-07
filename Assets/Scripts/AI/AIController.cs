using System.Collections;
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
        internal Rigidbody rigidbody;
        private bool _stunned;
        

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
            if (agent.enabled)
            {
                agent.SetDestination(target.transform.position);                
            }
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
