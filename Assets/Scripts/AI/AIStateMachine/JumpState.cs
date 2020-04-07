using UnityEngine;

namespace AI.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/JumpState")]
    public class JumpState : AiBaseState
    {
        public override void Enter()
        {
            Debug.Log("Test");
        }
    }
}
