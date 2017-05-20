using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.CubeModel;

public class PlayState1 : State
{
    [SerializeField]
    private CameraController _cameraController;

    private void OnEnable()
    {
        _cameraController.Initialize();
    }

    private Vector2 _startDragPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _startDragPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Vector2 direction = Input.mousePosition.ToVector2() - _startDragPosition;

            float horizontal = direction.x;
            float vertical = direction.y;

            float absHorizontal = Mathf.Abs(horizontal);
            float absVertical = Mathf.Abs(vertical);

            if (absHorizontal > absVertical)
            {
                if (horizontal > 0)
                {
                    _cameraController.AddMoveCommand(CameraCommand.LEFT);
                }
                else if (horizontal < 0)
                {
                    _cameraController.AddMoveCommand(CameraCommand.RIGHT);
                }
            }
            else if (absVertical > absHorizontal)
            {
                if (vertical > 0)
                {
                    _cameraController.AddMoveCommand(CameraCommand.UP);
                }
                else if (vertical < 0)
                {
                    _cameraController.AddMoveCommand(CameraCommand.DOWN);
                }
            }
        }
    }

    private void OnDisable()
    {
        _cameraController.ClearAllCommands();
    }
}
