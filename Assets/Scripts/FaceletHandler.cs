using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCubieDragCommand : UnityEvent<string> { }

public class FaceletHandler : MonoBehaviour
{
    [SerializeField]
    private string _plusXDragCommand;
    [SerializeField]
    private string _minusXDragCommand;
    [SerializeField]
    private string _plusYDragCommand;
    [SerializeField]
    private string _minusYDragCommand;
    [SerializeField]
    private string _plusZDragCommand;
    [SerializeField]
    private string _minusZDragCommand;

    private enum Direction
    {
        PlusX,
        MinusX,
        PlusY,
        MinusY,
        PlusZ,
        MinusZ
    }

    private bool _mouseUp = true;

    private Vector3 _startDragPosition;

    private OnCubieDragCommand _onDragCommand = new OnCubieDragCommand();
    public OnCubieDragCommand OnDragCommand { get { return _onDragCommand; } }

    private void OnMouseDown()
    {
        _mouseUp = false;
        _startDragPosition = GetMousePosition3D();
    }

    private void OnMouseUp()
    {
        _mouseUp = true;

        Vector3 mousePosition = GetMousePosition3D();
        Direction directon = DetermineDirection(mousePosition - _startDragPosition);
        switch (directon)
        {
            case Direction.PlusX:
                _onDragCommand.Invoke(_plusXDragCommand);
                break;
            case Direction.MinusX:
                _onDragCommand.Invoke(_minusXDragCommand);
                break;
            case Direction.PlusY:
                _onDragCommand.Invoke(_plusYDragCommand);
                break;
            case Direction.MinusY:
                _onDragCommand.Invoke(_minusYDragCommand);
                break;
            case Direction.PlusZ:
                _onDragCommand.Invoke(_plusZDragCommand);
                break;
            case Direction.MinusZ:
                _onDragCommand.Invoke(_minusZDragCommand);
                break;
        }
    }

    private Vector3 GetMousePosition3D()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Vector3.Distance(transform.position, Camera.main.transform.position);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        return mousePosition;
    }

    private Direction DetermineDirection(Vector3 dirtyDirection)
    {
        float absX = Mathf.Abs(dirtyDirection.x);
        float absY = Mathf.Abs(dirtyDirection.y);
        float absZ = Mathf.Abs(dirtyDirection.z);

        Direction direction = absX > absY ? absX > absZ ? Direction.PlusX : Direction.PlusZ : absY > absZ ? Direction.PlusY : Direction.PlusZ; ;

        switch (direction)
        {
            case Direction.PlusX:
                return dirtyDirection.x > 0 ? Direction.PlusX : Direction.MinusX;
            case Direction.PlusY:
                return dirtyDirection.y > 0 ? Direction.PlusY : Direction.MinusY;
            case Direction.PlusZ:
                return dirtyDirection.z > 0 ? Direction.PlusZ : Direction.MinusZ;
            default:
                return direction;
        }
    }

    private void OnDrawGizmos()
    {
        if (_mouseUp) return;

        Vector3 mousePosition = GetMousePosition3D();
        Vector3 dirtyDirection = mousePosition - _startDragPosition;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(_startDragPosition, mousePosition);

        GizmosUtils.DrawText(GUI.skin, dirtyDirection.ToString(), mousePosition, Color.magenta, 20, 10);
        GizmosUtils.DrawText(GUI.skin, DetermineDirection(dirtyDirection).ToString(), mousePosition, Color.magenta, 20, 30);
    }
}
