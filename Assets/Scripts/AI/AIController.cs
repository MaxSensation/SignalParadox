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
        private string _enemyType;

        private void Awake()
        {
            aihRenderer = transform.GetChild(0).GetComponent<Renderer>();
            rigidbody = GetComponent<Rigidbody>();
            _stateMachine = new StateMachine(this, states);
            target = GameObject.FindWithTag("Player");
            agent = GetComponent<NavMeshAgent>();
            moveSpeed = agent.speed;
            _enemyType = aihRenderer.name;
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

        internal void ActivateStun()
        {
            agent.enabled = false;
            _stunned = true;
            StartCoroutine("StunTime");
        }

        public bool IsStunned()
        {
            return _stunned;
        }

        public void Die()
        {
            if (_enemyType.Equals("BodyTrapperMesh"))
                gameObject.SetActive(false);
            else
                ActivateStun();
        }

        public Renderer GetRenderer()
        {
            return aihRenderer;
        }
    }
}
