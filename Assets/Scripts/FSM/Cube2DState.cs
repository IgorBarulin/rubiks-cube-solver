using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2DState : IState
{
    public Cube2DState(MainStateMachine mainStateMachine)
    {
        mainStateMachine.StopAllCoroutines();
        mainStateMachine.StartCoroutine(Process());
    }

    private IEnumerator Process()
    {
        CubeView2D cubeView2D = Instancer.Instance.CreateCubeView2D();
        cubeView2D.Initialize(Instancer.Instance.CreatePalette());
        yield return null;
    }
}
