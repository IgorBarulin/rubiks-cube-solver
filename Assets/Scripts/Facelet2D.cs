using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Facelet2D : UIBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image _view;

    public byte FaceletId { get; private set; }

    public byte ColorId { get; private set; }

    private OnPaintViewClick _onPaintViewClick = new OnPaintViewClick();
    public OnPaintViewClick OnClick
    {
        get { return _onPaintViewClick; }
    }

    public void Initialize(byte faceletId)
    {
        FaceletId = faceletId;
    }

    public void SetColor(byte colorId, Color color)
    {
        ColorId = colorId;
        _view.color = color;
    } 

    public void OnPointerClick(PointerEventData eventData)
    {
        _onPaintViewClick.Invoke(FaceletId);
    }
}
