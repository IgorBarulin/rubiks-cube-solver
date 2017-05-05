using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStateMachine : MonoBehaviour
{
    public IState State { get; set; }

    private void Awake()
    {
        State = new Cube2DState(this);
    }
}
