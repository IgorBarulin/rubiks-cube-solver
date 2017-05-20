using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitState : State
{
    [SerializeField]
    private State _playState;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            _stateMachine.State = _playState;
            _playState.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
	}
}
