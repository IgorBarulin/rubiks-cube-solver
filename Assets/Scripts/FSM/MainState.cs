using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FSM
{
    public class MainState
    {
        protected MainStateMachine _mainStateMachine;

        public MainState(MainStateMachine mainStateMachine)
        {
            _mainStateMachine = mainStateMachine;
        }

        public virtual void Start()
        {
            _mainStateMachine.StopAllCoroutines();
            _mainStateMachine.StartCoroutine(StartCoroutine());
        }

        public virtual void Finish()
        {
            _mainStateMachine.StopAllCoroutines();
            _mainStateMachine.StartCoroutine(FinishCoroutine());
        }

        protected virtual IEnumerator StartCoroutine()
        {
            yield return null;
        }

        protected virtual IEnumerator FinishCoroutine()
        {
            yield return null;
        }
    }
}