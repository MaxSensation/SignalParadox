using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovingDoorStateMachine
{
    [CreateAssetMenu(menuName = "MovingDoorStates/OpeningState")]
    public class OpeningDoorState : MovingDoorBaseState

    {
       [SerializeField] private IEnumerator _coroutine;

        public override void Enter()
        {
            Debug.Log("Enterd Opening State");
        }

        public override void Run()
        {
            _coroutine = MoveDoor(1.0f);
            //StartCoroutine(_coroutine);
            stateMachine.TransitionTo<OpenDoorState>();
        }

        //private void StartCoroutine(IEnumerator coroutine)
        //{
        //    coroutine.Reset();
        //}


        // every 2 seconds perform the print()
        private IEnumerator MoveDoor(float waitTime)
        {
            while (true)
            {
                yield return new WaitForSeconds(waitTime);
                MovingDoor.transform.position = Vector3.up;
            }
        }
    }
}