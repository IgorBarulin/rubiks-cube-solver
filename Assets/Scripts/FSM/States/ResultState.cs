using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CubeModel;
using UnityEngine;
using Assets.Scripts.CubeView;
using Assets.Scripts.Solver;
using UnityEngine.UI;

public class ResultState : State
{
    [SerializeField]
    private CubeView3D _cube3D;
    [SerializeField]
    private GameObject _resultPanel;
    [SerializeField]
    private Text _resultText;
    [SerializeField]
    private Button _returnButton;
    [SerializeField]
    private State _playState;

    public override void Enter(Cube cube)
    {
        base.Enter(cube);

        _cube3D.gameObject.SetActive(true);

        _returnButton.gameObject.SetActive(true);
        _returnButton.onClick.AddListener(TransitToPlayState);

        string solveCombo = Search.fullSolve(_cube, 20, 30000);
        Debug.Log(solveCombo);

        _resultPanel.gameObject.SetActive(true);
        _resultText.gameObject.SetActive(true);

        _resultText.text = solveCombo;

        _cube.Move(solveCombo);
        foreach (var cmd in solveCombo.Split(' '))
        {
            _cube3D.AddCommandInQueue(cmd);
        }
    }

    private void AddTo3DQ(string cmd)
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

    public override void Exit()
    {
        base.Exit();

        _cube3D.gameObject.SetActive(false);

        if (_returnButton)
        {
            _returnButton.gameObject.SetActive(false);
            _returnButton.onClick.RemoveListener(TransitToPlayState);
        }

        _resultPanel.gameObject.SetActive(false);
        _resultText.gameObject.SetActive(false);
    }

    private void TransitToPlayState()
    {
        _stateMachine.SwitchToState(_playState, _cube);
    }
}
