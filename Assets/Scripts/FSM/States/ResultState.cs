using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CubeModel;
using UnityEngine;
using Assets.Scripts.CubeView;
using Assets.Scripts.Solver;

public class ResultState : State
{
    [SerializeField]
    private CubeView3D _cube3D;
    [SerializeField]
    private ResultViewer _resultViewer;

    public override void Enter(Cube cube)
    {
        base.Enter(cube);

        _resultViewer.gameObject.SetActive(true);

        _cube3D.gameObject.SetActive(true);

        string solveCombo = Search.fullSolve(_cube, 20, 30000);
        Debug.Log(solveCombo);

        _resultViewer.Initialize(solveCombo.Split(' '));
        _resultViewer.OnCommand.AddListener(AddTo3DQ);
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

        _resultViewer.gameObject.SetActive(false);
        _resultViewer.OnCommand.RemoveListener(AddTo3DQ);

        _cube3D.gameObject.SetActive(false);
    }
}
