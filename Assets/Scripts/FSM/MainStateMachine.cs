using Assets.Scripts.FSM.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FSM
{
    public class MainStateMachine : MonoBehaviour
    {
        private MainState _state;
        public MainState State
        {
            get { return _state; }
            set
            {
                if (_state != null)
                    _state.Finish();
                _state = value;
                _state.Start();
            }
        }

        private void Awake()
        {
            State = new PlayState(this);
        }
    }
}
