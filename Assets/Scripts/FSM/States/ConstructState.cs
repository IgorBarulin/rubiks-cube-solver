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
    private State _validationState;
    [SerializeField]
    private Button _applyButton;

    public void Initialize(byte[] cubeConfig)
    {
        _constructor2d.gameObject.SetActive(true);
        _constructor2d.Initialize(cubeConfig);
    }

    private void OnEnable()
    {
        _applyButton.gameObject.SetActive(true);
        _applyButton.onClick.AddListener(TransitToValidationState);
    }

    private void OnDisable()
    {
        if (_constructor2d)
            _constructor2d.gameObject.SetActive(false);

        _applyButton.gameObject.SetActive(false);
        _applyButton.onClick.RemoveListener(TransitToValidationState);
    }

    private void TransitToValidationState()
    {
        _stateMachine.State = _validationState;
        _validationState.gameObject.SetActive(true);
        (_validationState as ValidationState).Initialize(_constructor2d.GetFacelets());
        gameObject.SetActive(false);
    }
}
