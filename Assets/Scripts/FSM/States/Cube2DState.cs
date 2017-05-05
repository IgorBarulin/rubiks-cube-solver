using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Tools;
using Assets.Scripts.CubeView;
using Assets.Scripts.CubeConstruct;

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
            Transform canvas = Prefabs.Instance.Canvas.transform;

            Palette palette = PoolManager.SpawnObject(Prefabs.Instance.Dictionary["Palette"]).GetComponent<Palette>();
            palette.transform.SetParent(canvas, false);
            yield return null;

            CubeView2D cubeView2D = PoolManager.SpawnObject(Prefabs.Instance.Dictionary["CubeView2D"]).GetComponent<CubeView2D>();
            cubeView2D.transform.SetParent(canvas, false);
            cubeView2D.Initialize(palette);
            yield return null;
        }
    }
}
