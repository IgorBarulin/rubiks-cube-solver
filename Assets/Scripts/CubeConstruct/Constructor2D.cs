using Assets.Scripts;
using Assets.Scripts.CubeModel;
using Assets.Scripts.CubeView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.CubeConstruct
{
    public class Constructor2D : UIBehaviour
    {
        [SerializeField]
        private Image[] _centers;
        [SerializeField]
        private Facelet2D[] _facelets2D;

        private Palette _palette;

        public void Initialize(Palette palette)
        {
            _palette = palette;

            for (byte cent = 0; cent < _centers.Length; cent++)
            {
                _centers[cent].color = _palette.Colors[cent];
            }

            for (byte id = 0; id < _facelets2D.Length; id++)
            {
                _facelets2D[id].Initialize(id);
                _facelets2D[id].OnClick.AddListener(SetFaceletColor);
            }
        }

        public byte[] GetFacelets()
        {
            byte[] facelets = new byte[Cube.FACELETS_AMOUNT];

            for (int i = 0; i < facelets.Length; i++)
            {
                facelets[i] = _facelets2D[i].ColorId;
            }

            return facelets;
        }

        private void SetFaceletColor(byte id)
        {
            _facelets2D[id].SetColor(_palette.SelectedId, _palette.SelectedColor);
        }
    }
}
