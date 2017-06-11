using Assets.Scripts.CubeConstruct;
using Assets.Scripts.CubeModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructState : State
{
    [SerializeField]
    private Constructor2D _constructor2d;
    [SerializeField]
    private GameObject _palette;
    [SerializeField]
    private State _validationState;
    [SerializeField]
    private Button _applyButton;

    public override void Enter(Cube cube)
    {
        base.Enter(cube);

        _constructor2d.gameObject.SetActive(true);
        _constructor2d.Initialize(_cube.GetFaceletColors());

        _palette.gameObject.SetActive(true);

        _applyButton.gameObject.SetActive(true);
        _applyButton.onClick.AddListener(TransitToValidationState);
    }

    public override void Exit()
    {
        base.Exit();

        if(_constructor2d)
            _constructor2d.gameObject.SetActive(false);

        _palette.gameObject.SetActive(false);

        _applyButton.gameObject.SetActive(false);
        _applyButton.onClick.RemoveListener(TransitToValidationState);
    }

    private void TransitToValidationState()
    {
        CubeFactory factory = new CubeFactory();
        _stateMachine.SwitchToState(_validationState, factory.CreateCube(_constructor2d.GetFacelets()));
    }
}
