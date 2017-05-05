using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.CubeView
{
    public class OnSwipe : UnityEvent<byte, Vector2> { }

    public class SwipeableFacelet2D : Facelet2D, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private Vector2 _startPosition = Vector2.zero;

        private OnSwipe _onSwipeEnd = new OnSwipe();

        public OnSwipe OnSwipeEnd
        {
            get { return _onSwipeEnd; }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = eventData.position;
            Debug.Log("Begin drag on position: " + _startPosition);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector2 direction = eventData.position - _startPosition;
            _onSwipeEnd.Invoke(FaceletId, direction);

            Debug.Log("End drag on position: " + eventData.position);
            Debug.Log("Drag direction: " + direction);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }
    }
}

