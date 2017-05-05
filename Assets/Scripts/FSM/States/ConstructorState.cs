using Assets.Scripts.CubeConstruct;
using Assets.Scripts.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts.FSM.States
{
    public class ConstructorState : IState
    {
        private MainStateMachine _mainStateMachine;

        private Palette _palette;
        private Constructor2D _constructor2D;

        public ConstructorState(MainStateMachine mainStateMachine)
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

            GameObject constructor2DPrefab = Prefabs.Instance.Dictionary["Constructor2D"];
            _constructor2D = PoolManager.SpawnObject(constructor2DPrefab).GetComponent<Constructor2D>();
            _constructor2D.transform.SetParent(canvas, false);
            _constructor2D.Initialize(_palette);
            yield return null;
        }

        private IEnumerator ExitProcess()
        {
            PoolManager.ReleaseObject(_palette.gameObject);
            PoolManager.ReleaseObject(_constructor2D.gameObject);
            yield return null;
        }
    }
}

