using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Tools;
using Assets.Scripts.CubeView;
using Assets.Scripts.CubeConstruct;
using System;
using UnityEngine.UI;

namespace Assets.Scripts.FSM.States
{
    public class PlayState : MainState
    {
        private CubeView2D _cubeView2D;

        private Button _button;

        private byte[] _facelets;

        public PlayState(MainStateMachine mainStateMachine, byte[] facelets = null) : base(mainStateMachine)
        {
            _facelets = facelets;

            Debug.Log("MainStateMachine: PlayState");
        }

        protected override IEnumerator StartCoroutine()
        {
            Transform canvas = Prefabs.Instance.Canvas.transform;
            yield return null;

            PaletteColors paletteColors = Resources.Load<PaletteColors>("DefaultColors");
            if (paletteColors == null)
                throw new Exception("PaletteColors not found at DefaultColors");
            yield return null;

            GameObject cubeView2DPrefab = Prefabs.Instance.Dictionary["CubeView2D"];
            _cubeView2D = PoolManager.SpawnObject(cubeView2DPrefab).GetComponent<CubeView2D>();
            _cubeView2D.transform.SetParent(canvas, false);
            _cubeView2D.Initialize(paletteColors.Colors, _facelets);
            yield return null;

            GameObject button = Prefabs.Instance.Dictionary["Button"];
            _button = PoolManager.SpawnObject(button).GetComponent<Button>();
            _button.transform.SetParent(canvas, false);
            _button.GetComponentInChildren<Text>().text = "Constructor";
            _button.onClick.AddListener(TransitToConstructor);
            yield return null;
        }

        protected override IEnumerator FinishCoroutine()
        {
            PoolManager.ReleaseObject(_cubeView2D.gameObject);
            _button.onClick.RemoveAllListeners();
            PoolManager.ReleaseObject(_button.gameObject);
            yield return null;
        }

        private void TransitToConstructor()
        {
        }
    }
}