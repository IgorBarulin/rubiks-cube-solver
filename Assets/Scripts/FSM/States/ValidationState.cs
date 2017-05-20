using Assets.Scripts.CubeModel;
using Assets.Scripts.Solver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FSM.States
{
    public class ValidationState : MainState
    {
        private byte[] _facelets;

        public ValidationState(MainStateMachine mainStateMachine, byte[] facelets) : base(mainStateMachine)
        {
            _facelets = facelets;

            Debug.Log("MainStateMachine: ValidationState");
        }

        protected override IEnumerator StartCoroutine()
        {
            yield return null;
        }

        protected override IEnumerator FinishCoroutine()
        {
            return base.FinishCoroutine();
        }
    }
}