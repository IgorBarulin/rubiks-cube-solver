using Assets.Scripts.CubeModel;
using UnityEngine;

namespace Assets.Scripts.FSM
{
    public class MainStateMachine : MonoBehaviour
    {
        public State State;

        private void Start()
        {
            CubeFactory factory = new CubeFactory();
            State.Enter(factory.CreateCube(null));
        }

        public void SwitchToState(State state, Cube cube)
        {
            State.Exit();
            State = state;
            State.Enter(cube);
        }
    }
}
