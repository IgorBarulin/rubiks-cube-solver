using Assets.Scripts.CubeModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitState : State
{
    [SerializeField]
    private State _playState;

    public override void Enter(Cube cube)
    {
        base.Enter(cube);

        _stateMachine.SwitchToState(_playState, _cube);
    }
}
