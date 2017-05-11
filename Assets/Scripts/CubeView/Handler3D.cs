using Assets.Scripts.CubeView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CubeView
{
    public class Handler3D : MonoBehaviour
    {
        private Vector2 _startDragPosition;

        public byte FaceletId { get; private set; }

        private OnSwipe _onSwipeEnd = new OnSwipe();
        public OnSwipe OnSwipEnd
        {
            get { return _onSwipeEnd; }
        }

        public void Initialize(byte faceletId)
        {
            FaceletId = faceletId;
        }

        private void OnMouseDown()
        {
            _startDragPosition = Input.mousePosition;
            Debug.Log("Mouse down " + Input.mousePosition);
        }

        private void OnMouseUp()
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 direction = mousePosition - _startDragPosition;
            _onSwipeEnd.Invoke(FaceletId, direction);
            Debug.Log("Mouse up " + Input.mousePosition);
        }
    }
}