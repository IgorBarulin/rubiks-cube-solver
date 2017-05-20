using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.CubeModel;
using Assets.Scripts.CubeView;
using UnityEngine.UI;

public class PlayState1 : State
{
    [SerializeField]
    private CameraController _cameraController;
    [SerializeField]
    private CubeView3D _cube3D;
    [SerializeField]
    private Button _constructButton;
    [SerializeField]
    private State _constructState;

    private Cube _cube;
    private Vector2 _startDragPosition;


    public void Initialize(Cube cube)
    {
        _cube = cube;
        _cube3D.SetConfiguration(_cube.GetFaceletColors());
        _cube3D.OnFaceletDrag.AddListener(_cube.Move);
    }

    private void OnEnable()
    {
        _cameraController.Initialize();
        _cube3D.gameObject.SetActive(true);
        _constructButton.gameObject.SetActive(true);
        _constructButton.onClick.AddListener(TransitToConstructState);
    }

    private void OnDisable()
    {
        _cameraController.ClearAllCommands();
        _cube3D.gameObject.SetActive(false);
        _cube3D.OnFaceletDrag.RemoveListener(_cube.Move);
        _constructButton.gameObject.SetActive(false);
        _constructButton.onClick.RemoveListener(TransitToConstructState);
    }

    private void TransitToConstructState()
    {
        _stateMachine.State = _constructState;
        _constructState.gameObject.SetActive(true);
        (_constructState as ConstructState).Initialize(_cube.GetFaceletColors());
        gameObject.SetActive(false);
    }

    private void Update()
    {
        HandleCameraMoving();
    }

    private void HandleCameraMoving()
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
}
