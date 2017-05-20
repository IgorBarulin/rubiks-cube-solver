using Assets.Scripts.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    protected MainStateMachine _stateMachine;

    private void Awake()
    {
        _stateMachine = gameObject.GetComponentInParent<MainStateMachine>();
    }
}
