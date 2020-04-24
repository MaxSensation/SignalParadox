﻿using AI;
using UnityEngine;

namespace PlayerStateMachine
{
    [CreateAssetMenu(menuName = "PlayerState/MeleeState")]
    public class MeleeState : PlayerBaseState
    {
        [SerializeField] private float hitDistance;

        public override void Enter()
        {
            Melee();
            Debug.Log("Entered Melee");
            stateMachine.UnStackState();
        }

        private void Melee()
        {
            Player.OnMelee();
        }
    }
}