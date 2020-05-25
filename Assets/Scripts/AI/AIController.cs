//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public abstract class AIController : MonoBehaviour
    {
        [SerializeField] private State[] states;
        [SerializeField] internal Transform[] waypoints;
        [SerializeField] internal LayerMask visionMask;
        [SerializeField] private float stunnedTime = 0.5f;
        
        protected StateMachine stateMachine;
        protected bool isStunned;
        internal GameObject target;
        internal NavMeshAgent agent;
        internal Rigidbody aiRigidbody;
        internal bool isDead;
        private WaitForSeconds stunTimeSeconds;
        internal CapsuleCollider AiCollider { get; private set; }

        protected void Awake()
        {
            stunTimeSeconds = new WaitForSeconds(stunnedTime);
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
        
        internal void ActivateStun()
        {
            isStunned = true;
            StartCoroutine(StunTime());
        }
        
        private IEnumerator StunTime()
        {
            yield return stunTimeSeconds;
            isStunned = false;
        }

        protected abstract void Die();

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
