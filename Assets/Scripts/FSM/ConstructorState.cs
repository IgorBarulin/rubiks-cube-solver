using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructorState : IState
{
    public ConstructorState(MainStateMachine mainStateMachine)
    {
        mainStateMachine.StopAllCoroutines();
        mainStateMachine.StartCoroutine(Process());
    }

    private IEnumerator Process()
    {
        var palette = Instancer.Instance.CreatePalette();
        var constructor2D = Instancer.Instance.CreateConstructor2D();
        constructor2D.Initialize(palette);
        yield return null;
    }
}
