using Assets.Scripts.CubeModel;
using Assets.Scripts.Solver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidationState : State
{
    [SerializeField]
    private State _playState;
    [SerializeField]
    private State _constructState;
    [SerializeField]
    private MessageBox _messageBox;

    public override void Enter(Cube cube)
    {
        base.Enter(cube);

        byte[] cubeConfig = cube.GetFaceletColors();

        int[] facelets = new int[6];

        for (int i = 0; i < cubeConfig.Length; i++)
        {
            facelets[cubeConfig[i]]++;
        }

        bool valid = true;
        for (int i = 0; i < facelets.Length; i++)
        {
            if (facelets[i] != 8)
            {
                valid = false;
                break;
            }
        }

        if (valid)
        {
            TransitToPlayState();
        }
        else
        {
            _messageBox.Show("Invalid cube");
            _messageBox.OkButton.onClick.AddListener(TransitToConstructState);
        }
    }


    public override void Exit()
    {
        base.Exit();

        _messageBox.OkButton.onClick.RemoveAllListeners();
    }

    private void TransitToPlayState()
    {
        _stateMachine.SwitchToState(_playState, _cube);
    }

    private void TransitToConstructState()
    {
        _stateMachine.SwitchToState(_constructState, _cube);
    }
}
