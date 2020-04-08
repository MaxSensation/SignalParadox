using System;
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
        private Renderer aihRenderer;
        internal GameObject target;
        internal NavMeshAgent agent;
        internal Rigidbody rigidbody;
        internal CapsuleCollider _collider;


        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider>();
            aihRenderer = transform.GetChild(0).GetComponent<Renderer>();
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
            agent.enabled = true;
            _stunned = false;
        }
        
        private IEnumerator OnlyStunTime()
        {
            yield return new WaitForSeconds(3);
            _stunned = false;
        }

        internal void ActivateStun()
        {
            agent.enabled = false;
            _stunned = true;
            StartCoroutine("StunTime");
        }

        internal void ActivateOnlyStun()
        {
            _stunned = true;
            StartCoroutine("OnlyStunTime");
        }

        public bool IsStunned()
        {
            return _stunned;
        }

        public void Die()
        {
            _stunned = true;
        }

        public Renderer GetRenderer()
        {
            return aihRenderer;
        }

        public CapsuleCollider GetCollider()
        {
            return _collider;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward);
        }
    }
}
