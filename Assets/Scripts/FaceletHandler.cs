using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnFaceletDrag : UnityEvent<string> { }

public enum Axis { X, Y, Z }

[Serializable]
public class AxisCommand
{
    public Axis Axis;

    public string PositiveCommand;
    public string NegativeCommand;
}

public class FaceletHandler : MonoBehaviour
{
    [SerializeField]
    private AxisCommand _axisCommand1;
    [SerializeField]
    private AxisCommand _axisCommand2;

    private bool _mouseUp = true;

    private Vector3 _startDragPosition;

    private OnFaceletDrag _onFaceletDrag = new OnFaceletDrag();
    public OnFaceletDrag OnFaceletDrag { get { return _onFaceletDrag; } }

    private void OnMouseDown()
    {
        _mouseUp = false;
        _startDragPosition = GetMousePosition3D();
    }

    private void OnMouseUp()
    {
        _mouseUp = true;

        Vector3 mousePosition = GetMousePosition3D();
        Vector3 direction = transform.worldToLocalMatrix.MultiplyVector(mousePosition - _startDragPosition);
        Dictionary<Axis, float> customV3 = new Dictionary<Axis, float>()
        {
            { Axis.X, direction.x }, { Axis.Y, direction.y }, { Axis.Z, direction.z }
        };

        AxisCommand axisCommand = Mathf.Abs(customV3[_axisCommand1.Axis]) > Mathf.Abs(customV3[_axisCommand2.Axis]) ? _axisCommand1 : _axisCommand2;
        string command = customV3[axisCommand.Axis] > 0 ? axisCommand.PositiveCommand : axisCommand.NegativeCommand;
        _onFaceletDrag.Invoke(command);
    }

    private Vector3 GetMousePosition3D()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Vector3.Distance(transform.position, Camera.main.transform.position);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        return mousePosition;
    }

    private void OnDrawGizmos()
    {
        if (_mouseUp) return;

        Vector3 mousePosition = GetMousePosition3D();
        Vector3 dirtyDirection = mousePosition - _startDragPosition;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(_startDragPosition, mousePosition);

        GizmosUtils.DrawText(GUI.skin, dirtyDirection.ToString(), mousePosition, Color.magenta, 20, 10);
    }
}
