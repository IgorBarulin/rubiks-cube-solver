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
        PaletteColors _paletteColors;
        [SerializeField]
        private Paint[] _paints;

        public PaletteColors PaletteColors
        {
            get { return _paletteColors; }
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

            for (byte i = 0; i < _paletteColors.Colors.Length; i++)
            {
                _paints[i].Initialize(i, _paletteColors.Colors[i]);
                _paints[i].OnPaintViewClick.AddListener(SelectColor);
            }

            _selectedColor = _paletteColors.Colors[0];
        }

        private void SelectColor(byte id)
        {
            _selectedId = id;
            _selectedColor = _paletteColors.Colors[id];
        }
    }
}