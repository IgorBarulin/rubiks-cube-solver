using Assets.Scripts.FSM.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FSM
{
    public class MainStateMachine : MonoBehaviour
    {
        public IState State { get; set; }

        private void Awake()
        {
            State = new Cube2DState(this);
        }
    }
}
