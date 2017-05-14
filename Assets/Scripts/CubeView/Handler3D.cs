using Assets.Scripts.CubeView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CubeView
{
    public class Handler3D : MonoBehaviour
    {
        private Vector3 _startDragPosition;

        private bool _mouseDown;

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
            _mouseDown = true;
            _startDragPosition = Input.mousePosition;
            float distanceFromCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
            _startDragPosition.z = distanceFromCamera;
            _startDragPosition = Camera.main.ScreenToWorldPoint(_startDragPosition);
        }

        private void OnMouseUp()
        {
            _mouseDown = false;

            //Vector3 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //Vector3 direction = mousePosition - _startDragPosition;
            //_onSwipeEnd.Invoke(FaceletId, direction);
            //Debug.Log("Mouse up " + Input.mousePosition);
        }

        private void OnDrawGizmos()
        {
            if (_mouseDown)
            {
                Vector3 mousePosition = Input.mousePosition;
                float distanceFromCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
                mousePosition.z = distanceFromCamera;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                Gizmos.DrawLine(_startDragPosition, mousePosition);
                Vector3 direction = mousePosition - _startDragPosition;
                GizmosUtils.DrawText(GUI.skin, direction.ToString(), mousePosition, Color.white, 20, 10);

                float absX = Mathf.Abs(direction.x);
                float absY = Mathf.Abs(direction.y);
                float absZ = Mathf.Abs(direction.z);

                Vector3 v;
                float max;
                if (absX > absY)
                {
                    max = absX;
                    v = Vector3.right;
                }
                else
                {
                    max = absY;
                    v = Vector3.up;
                }
                if (max < absZ)
                {
                    v = Vector3.forward;
                }

                string text = "";
                if (v == Vector3.right)
                {
                    text = "(X)";
                }
                else if (v == Vector3.up)
                {
                    text = "(Y)";
                }
                else if (v == Vector3.forward)
                {
                    text = "(Z)";
                }

                GizmosUtils.DrawText(GUI.skin, text, mousePosition, Color.white, 20, 30);
            }
        }
    }
}