using Assets.Scripts.CubeModel;
using Assets.Scripts.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    protected MainStateMachine _stateMachine;
    protected Cube _cube;

    private void Awake()
    {
        _stateMachine = gameObject.GetComponentInParent<MainStateMachine>();
    }

    public virtual void Enter(Cube cube)
    {
        _cube = cube;
    }

    public virtual void Exit()
    {
    }
}
