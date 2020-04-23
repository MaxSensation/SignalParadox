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
        private string _enemyType;
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
            _enemyType = aihRenderer.name;
            _stateMachine = new StateMachine(this, states);
        }

        private void Update()
        {
            agent.speed = moveSpeed;
            _stateMachine.Run();
        }
        private IEnumerator StunTime()
        {
            yield return new WaitForSeconds(3);
            agent.isStopped = true;
            _stunned = false;
        }
        
        private IEnumerator OnlyStunTime()
        {
            yield return new WaitForSeconds(3);
            _stunned = false;
        }

        private IEnumerator ChargeTime()
        {
            yield return new WaitForSeconds(2);
            agent.isStopped = false;
            hasChargedUp = true;
            //StopCoroutine("ChargeTime");
        }

        internal void ActivateStun()
        {
            agent.isStopped = false;
            _stunned = true;
            StartCoroutine("StunTime");
        }

        internal void ChargeUp()
        {
            agent.isStopped = true;
            hasChargedUp = false;
            StartCoroutine("ChargeTime");
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
            if (_enemyType.Equals("BodyTrapperMesh")){
                // gameObject.SetActive(false);
                isDead = true;
            }
            else
                ActivateStun();
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

        internal void TouchingPlayer()
        {
            Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized, out var hit, 1f);
            if (hit.collider && hit.collider.CompareTag("Player"))
                onTrappedPlayer?.Invoke();
        }

        public void PlayerCrushed()
        {
            if (rigidbody.velocity.magnitude <= 0.001f)
            {
                if (target.transform.parent == transform)
                    onCrushedPlayer?.Invoke();
                target.transform.parent = null;
                agent.enabled = true;
                ActivateOnlyStun();
                _stateMachine.TransitionTo<Charger.AIStateMachine.HuntState>();
            }
        }
    }
}
