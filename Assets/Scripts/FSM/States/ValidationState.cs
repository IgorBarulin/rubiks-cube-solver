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
            _mainStateMachine.State = new PlayState(_mainStateMachine, _facelets);
            yield return null;
        }

        protected override IEnumerator FinishCoroutine()
        {
            return base.FinishCoroutine();
        }
    }
}