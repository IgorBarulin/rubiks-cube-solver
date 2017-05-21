using Assets.Scripts.CubeModel;
using Assets.Scripts.Solver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidationState : State
{
    [SerializeField]
    private State _playState;
    [SerializeField]
    private State _constructState;
    [SerializeField]
    private MessageBox _messageBox;

    private byte[] _cubeConfig;
    private CubeFactory _cubeFactory = new CubeFactory();

    public void Initialize(byte[] cubeConfig)
    {
        _cubeConfig = cubeConfig;

        int[] facelets = new int[6];

        for (int i = 0; i < cubeConfig.Length; i++)
        {
            facelets[cubeConfig[i]]++;
        }

        bool valid = true;
        for (int i = 0; i < facelets.Length; i++)
        {
            if (facelets[i] != 8)
            {
                valid = false;
                break;
            }
        }

        if (valid)
        {
            TransitToPlayState();
        }
        else
        {
            _messageBox.Show("Invalid cube");
            _messageBox.OkButton.onClick.AddListener(TransitToConstructState);
        }
    }

    private void OnDisable()
    {
        _messageBox.OkButton.onClick.RemoveAllListeners();        
    }

    private void TransitToPlayState()
    {
        _stateMachine.State = _playState;
        _playState.gameObject.SetActive(true);
        (_playState as PlayState).Initialize(_cubeFactory.CreateCube(_cubeConfig));
        gameObject.SetActive(false);
    }

    private void TransitToConstructState()
    {
        _stateMachine.State = _constructState;
        _constructState.gameObject.SetActive(true);
        (_constructState as ConstructState).Initialize(_cubeConfig);
        gameObject.SetActive(false);
    }
}
