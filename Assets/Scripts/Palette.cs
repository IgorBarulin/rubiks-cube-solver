using Assets.Scripts.CubeModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class Palette : UIBehaviour
{
    [SerializeField]
    private byte _selectedId = 0;
    [SerializeField]
    private Color _selectedColor;
    [SerializeField]
    private Color[] _colors;
    [SerializeField]
    private Paint[] _paints;

    public Color[] Colors
    {
        get { return _colors; }
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

        for (byte i = 0; i < _colors.Length; i++)
        {
            _paints[i].Initialize(i, _colors[i]);
            _paints[i].OnPaintViewClick.AddListener(SelectColor);
        }

        _selectedColor = _colors[0];
    }

    private void SelectColor(byte id)
    {
        _selectedId = id;
        _selectedColor = _colors[id];
    }
}