using Assets.Scripts.CubeConstruct;
using Assets.Scripts.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Assets.Scripts.FSM.States
{
    public class ConstructorState : MainState
    {
        private Palette _palette;
        private Constructor2D _constructor2D;
        private byte[] _initialFacelets;

        private Button _button;

        public ConstructorState(MainStateMachine mainStateMachine, byte[] initialFacelets = null) : base(mainStateMachine)
        {
            _initialFacelets = initialFacelets;

            Debug.Log("MainStateMachine: ConstructorState");
        }

        protected override IEnumerator StartCoroutine()
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
            _constructor2D.Initialize(_initialFacelets);
            yield return null;

            GameObject button = Prefabs.Instance.Dictionary["Button"];
            _button = PoolManager.SpawnObject(button).GetComponent<Button>();
            _button.transform.SetParent(canvas, false);
            _button.GetComponentInChildren<Text>().text = "Complete";
            _button.onClick.AddListener(TransitToValidation);
            yield return null;
        }

        protected override IEnumerator FinishCoroutine()
        {
            PoolManager.ReleaseObject(_palette.gameObject);
            PoolManager.ReleaseObject(_constructor2D.gameObject);
            yield return null;
            _button.onClick.RemoveAllListeners();
            PoolManager.ReleaseObject(_button.gameObject);
            yield return null;
        }

        private void TransitToValidation()
        {
        }
    }
}