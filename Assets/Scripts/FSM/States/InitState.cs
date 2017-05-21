using Assets.Scripts.CubeModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitState : State
{
    [SerializeField]
    private State _playState;

    private void OnEnable()
    {
        _stateMachine.State = _playState;
        _playState.gameObject.SetActive(true);
        (_playState as PlayState).Initialize(new CubeFactory().CreateCube(null));
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {

    }
}
