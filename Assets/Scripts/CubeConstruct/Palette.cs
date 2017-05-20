using Assets.Scripts.CubeModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

namespace Assets.Scripts.CubeConstruct
{
    public class Palette : UIBehaviour
    {
        [SerializeField]
        private byte _selectedId = 0;
        [SerializeField]
        private Color _selectedColor;
        [SerializeField]
        CubeColorSheme _colorSheme;
        [SerializeField]
        private Paint[] _paints;

        public CubeColorSheme ColorSheme
        {
            get { return _colorSheme; }
        }

        public byte SelectedId
        {
            get { return _selectedId; }
        }

        public Color SelectedColor
        {
            get { return _selectedColor; }
        }

        protected override void Awake()
        {
            base.Awake();

            for (byte i = 0; i < 6; i++)
            {
                _paints[i].Initialize(i, _colorSheme.GetColor(i));
                _paints[i].OnPaintViewClick.AddListener(SelectColor);
            }

            _selectedColor = _colorSheme.GetColor(0);
        }

        private void SelectColor(byte id)
        {
            _selectedId = id;
            _selectedColor = _colorSheme.GetColor(id);
        }
    }
}