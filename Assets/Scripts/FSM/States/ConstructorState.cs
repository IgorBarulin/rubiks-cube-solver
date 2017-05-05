using Assets.Scripts.CubeConstruct;
using Assets.Scripts.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FSM.States
{
    public class ConstructorState : IState
    {
        public ConstructorState(MainStateMachine mainStateMachine)
        {
            mainStateMachine.StopAllCoroutines();
            mainStateMachine.StartCoroutine(Process());
        }

        private IEnumerator Process()
        {
            Transform canvas = Prefabs.Instance.Canvas.transform;

            var palette = PoolManager.SpawnObject(Prefabs.Instance.Dictionary["Palette"]).GetComponent<Palette>();
            palette.transform.SetParent(canvas, false);
            yield return null;

            var constructor2D = PoolManager.SpawnObject(Prefabs.Instance.Dictionary["Constructor2D"]).GetComponent<Constructor2D>();
            constructor2D.transform.SetParent(canvas, false);
            constructor2D.Initialize(palette);
            yield return null;
        }
    }
}

