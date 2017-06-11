using Assets.Scripts.CubeModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitState : State
{
    [SerializeField]
    private State _playState;
    [SerializeField]
    private Button _quitButton;

    public override void Enter(Cube cube)
    {
        base.Enter(cube);

        _quitButton.onClick.AddListener(() => Application.Quit());

        _stateMachine.SwitchToState(_playState, _cube);
    }
}
