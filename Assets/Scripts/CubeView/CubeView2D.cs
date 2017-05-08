using Assets.Scripts.CubeConstruct;
using Assets.Scripts.CubeModel;
using Assets.Scripts.Solver;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.CubeView
{
    public class CubeView2D : UIBehaviour
    {
        [SerializeField]
        private Image[] _centers;

        [SerializeField]
        private SwipeableFacelet2D[] _facelets;

        private Cube _cube;

        private Color[] _colors;

        public void Initialize(Color[] colors, byte[] initialFacelets)
        {
            _colors = colors;

            for (byte cent = 0; cent < _centers.Length; cent++)
            {
                _centers[cent].color = _colors[cent];
            }

            for (byte i = 0; i < _facelets.Length; i++)
            {
                _facelets[i].Initialize(i);
                _facelets[i].OnSwipeEnd.AddListener(Move);
            }

            _cube = new CubeFactory().CreateCube(initialFacelets);
            UpdateView(_cube);
        }

        private void Move(byte faceletId, Vector2 direction)
        {
            string mv = SwipeConverter.GetMove(faceletId, direction);
            if (mv == null) return;
            _cube.Move(mv);
            UpdateView(_cube);
        }

        public byte[] GetFacelets()
        {
            byte[] facelets = new byte[Cube.FACELETS_AMOUNT];

            for (int i = 0; i < facelets.Length; i++)
            {
                facelets[i] = _facelets[i].ColorId;
            }

            return facelets;
        }

        public void UpdateView(Cube cube)
        {
            byte[] faceletColors = cube.GetFaceletColors();

            for (int i = 0; i < Cube.FACELETS_AMOUNT; i++)
            {
                _facelets[i].SetColor(faceletColors[i], _colors[faceletColors[i]]);
            }
        }
    }
}

