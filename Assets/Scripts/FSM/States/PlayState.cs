using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.CubeModel;
using Assets.Scripts.CubeView;
using UnityEngine.UI;
using Assets.Scripts.Solver;

public class PlayState : State
{
    [SerializeField]
    private CameraController _cameraController;
    [SerializeField]
    private CubeView3D _cube3D;
    [SerializeField]
    private Button _constructButton;
    [SerializeField]
    private Button _solveButton;
    [SerializeField]
    private State _constructState;
    [SerializeField]
    private State _shuffleState;
    [SerializeField]
    private Button _shuffleButton;

    private Vector2 _startDragPosition;


    public override void Enter(Cube cube)
    {
        base.Enter(cube);

        _cube3D.SetConfiguration(_cube.GetFaceletColors());
        _cube3D.OnFaceletDrag.AddListener(_cube.Move);

        _cameraController.Initialize();

        _cube3D.gameObject.SetActive(true);

        _constructButton.gameObject.SetActive(true);
        _constructButton.onClick.AddListener(TransitToConstructState);

        _solveButton.gameObject.SetActive(true);
        _solveButton.onClick.AddListener(Solve);

        _shuffleButton.gameObject.SetActive(true);
        _shuffleButton.onClick.AddListener(TransitToShuffleState);
    }

    public override void Exit()
    {
        base.Exit();

        if (_cameraController)
        {
            _cameraController.ClearAllCommands();
        }

        if (_cube3D)
        {
            _cube3D.gameObject.SetActive(false);
            _cube3D.OnFaceletDrag.RemoveListener(_cube.Move);
        }

        if (_constructButton)
        {
            _constructButton.gameObject.SetActive(false);
            _constructButton.onClick.RemoveListener(TransitToConstructState);
        }

        if (_solveButton)
        {
            _solveButton.gameObject.SetActive(false);
            _solveButton.onClick.RemoveListener(Solve);
        }

        if (_shuffleButton)
        {
            _shuffleButton.gameObject.SetActive(false);
            _shuffleButton.onClick.RemoveListener(TransitToShuffleState);
        }
    }

    private void TransitToConstructState()
    {
        _stateMachine.SwitchToState(_constructState, _cube);
    }

    private void TransitToShuffleState()
    {
        _stateMachine.SwitchToState(_shuffleState, _cube);
    }

    private void Solve()
    {
        string solveCombo = Search.fullSolve(_cube, 20, 10000);
        foreach (var cmd in solveCombo.Split(' '))
        {
            switch (cmd)
            {
                case "U2":
                    _cube3D.AddCommandInQueue("U");
                    _cube3D.AddCommandInQueue("U");
                    break;
                case "R2":
                    _cube3D.AddCommandInQueue("R");
                    _cube3D.AddCommandInQueue("R");
                    break;
                case "F2":
                    _cube3D.AddCommandInQueue("F");
                    _cube3D.AddCommandInQueue("F");
                    break;
                case "L2":
                    _cube3D.AddCommandInQueue("L");
                    _cube3D.AddCommandInQueue("L");
                    break;
                case "B2":
                    _cube3D.AddCommandInQueue("B");
                    _cube3D.AddCommandInQueue("B");
                    break;
                case "D2":
                    _cube3D.AddCommandInQueue("D");
                    _cube3D.AddCommandInQueue("D");
                    break;
                default:
                    _cube3D.AddCommandInQueue(cmd);
                    break;
            }
        }
        Debug.Log(solveCombo);
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
