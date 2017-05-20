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
        [SerializeField]
        private Palette _palette;
        [SerializeField]
        private Button _applyButton;

        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < _facelets2D.Length; i++)
            {
                _facelets2D[i].OnClick.AddListener(SetFaceletColor);
            }
        }

        public void Initialize(byte[] initialFacelets)
        {
            for (byte i = 0; i < _centers.Length; i++)
            {
                _centers[i].color = _palette.ColorSheme.GetColor(i);
            }

            if (initialFacelets == null)
            {
                for (byte i = 0; i < _facelets2D.Length; i++)
                {
                    _facelets2D[i].Initialize(i);
                }
            }
            else
            {
                for (byte i = 0; i < initialFacelets.Length; i++)
                {
                    _facelets2D[i].Initialize(i);
                    _facelets2D[i].SetColor(initialFacelets[i], _palette.ColorSheme.GetColor(initialFacelets[i]));
                }
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
