using Assets.Scripts.CubeModel;
using Assets.Scripts.CubeView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShuffleState : State
{
    [SerializeField]
    private CubeView3D _cube3D;
    [SerializeField]
    private Text _shuffleCountText;
    [SerializeField]
    private GameObject _shuffleBg;
    [SerializeField]
    private Button _stopButton;
    [SerializeField]
    private State _playState;

    [SerializeField]
    private float _generationPause;

    private int _shuffleCount;

    private static readonly string[] _validMoves = "U U' R R' F F' D D' L L' B B'".Split(' ');

    public override void Enter(Cube cube)
    {
        base.Enter(cube);

        _cube3D.OnFaceletDrag.AddListener(_cube.Move);

        _cube3D.gameObject.SetActive(true);

        _shuffleCount = 0;
        _shuffleCountText.text = _shuffleCount.ToString();

        _shuffleCountText.gameObject.SetActive(true);
        _shuffleBg.gameObject.SetActive(true);

        _stopButton.gameObject.SetActive(true);
        _stopButton.onClick.AddListener(TransitToPlayState);

        StartCoroutine(ShuffleGenerator(_generationPause));
    }

    public override void Exit()
    {
        base.Exit();

        StopAllCoroutines();

        _shuffleCountText.gameObject.SetActive(false);
        _shuffleBg.gameObject.SetActive(false);

        _stopButton.gameObject.SetActive(false);
        _stopButton.onClick.RemoveListener(TransitToPlayState);

        _cube3D.OnFaceletDrag.RemoveListener(_cube.Move);
    }

    private IEnumerator ShuffleGenerator(float pause)
    {
        while (true)
        {
            string move = _validMoves[Random.Range(0, _validMoves.Length)];
            _cube3D.AddCommandInQueue(move);

            _shuffleCount++;
            _shuffleCountText.text = _shuffleCount.ToString();

            Debug.Log("Move: " + move);

            yield return new WaitForSeconds(pause);
        }
    }

    private void TransitToPlayState()
    {
        _stateMachine.SwitchToState(_playState, _cube);
    }
}
