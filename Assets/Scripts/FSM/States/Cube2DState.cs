using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Tools;
using Assets.Scripts.CubeView;

namespace Assets.Scripts.FSM.States
{
    public class Cube2DState : IState
    {
        private MainStateMachine mainStateMachine;

        public Cube2DState(MainStateMachine mainStateMachine)
        {
            mainStateMachine.StopAllCoroutines();
            mainStateMachine.StartCoroutine(Process());
        }

        private IEnumerator Process()
        {
            CubeView2D cubeView2D = Instancer.Instance.CreateCubeView2D();
            cubeView2D.Initialize(Instancer.Instance.CreatePalette());
            yield return null;
        }
    }
}
