using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnPaintViewClick : UnityEvent<byte> { }

public class Paint : UIBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image _view;

    private byte _id;

    private OnPaintViewClick _onPaintViewClick = new OnPaintViewClick();
    public OnPaintViewClick OnPaintViewClick
    {
        get { return _onPaintViewClick; }
    }

    public void Initialize(byte id, Color color)
    {
        _id = id;
        _view.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onPaintViewClick.Invoke(_id);
    }
}
