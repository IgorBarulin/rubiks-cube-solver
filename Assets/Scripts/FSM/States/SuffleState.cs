using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FSM.States
{
    public class SuffleState : MainState
    {
        public SuffleState(MainStateMachine mainStateMachine) : base(mainStateMachine)
        {
        }

        protected override IEnumerator StartCoroutine()
        {
            return base.StartCoroutine();
        }

        protected override IEnumerator FinishCoroutine()
        {
            return base.FinishCoroutine();
        }
    }
}