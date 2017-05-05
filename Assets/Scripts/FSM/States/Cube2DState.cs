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
        private MainStateMachine _mainStateMachine;

        private Palette _palette;
        private CubeView2D _cubeView2D;

        public Cube2DState(MainStateMachine mainStateMachine)
        {
            _mainStateMachine = mainStateMachine;

            if (_mainStateMachine.State != null)
                _mainStateMachine.State.Exit();

            _mainStateMachine.StopAllCoroutines();
            _mainStateMachine.StartCoroutine(EnterProcess());
        }

        public void Exit()
        {
            _mainStateMachine.StopAllCoroutines();
            _mainStateMachine.StartCoroutine(ExitProcess());
        }

        private IEnumerator EnterProcess()
        {
            Transform canvas = Prefabs.Instance.Canvas.transform;
            yield return null;

            GameObject palettePrefab = Prefabs.Instance.Dictionary["Palette"];
            _palette = PoolManager.SpawnObject(palettePrefab).GetComponent<Palette>();
            _palette.transform.SetParent(canvas, false);
            yield return null;

            GameObject cubeView2DPrefab = Prefabs.Instance.Dictionary["CubeView2D"];
            _cubeView2D = PoolManager.SpawnObject(cubeView2DPrefab).GetComponent<CubeView2D>();
            _cubeView2D.transform.SetParent(canvas, false);
            _cubeView2D.Initialize(_palette);
            yield return null;
        }

        private IEnumerator ExitProcess()
        {
            PoolManager.ReleaseObject(_palette.gameObject);
            PoolManager.ReleaseObject(_cubeView2D.gameObject);
            yield return null;
        }
    }
}
