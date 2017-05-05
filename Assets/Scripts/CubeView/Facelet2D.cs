using Assets.Scripts.CubeConstruct;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.CubeView
{
    public class Facelet2D : UIBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Image _view;

        public byte FaceletId { get; private set; }

        public byte ColorId { get; private set; }

        private OnFaceletClick _onPaintViewClick = new OnFaceletClick();
        public OnFaceletClick OnClick
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

}
