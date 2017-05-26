using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResultViewer : UIBehaviour
{
    [SerializeField]
    private Scroller _scroller;
    [SerializeField]
    private Button _playButton;
    [SerializeField]
    private Button _stopButton;
    [SerializeField]
    private Button _prevButton;
    [SerializeField]
    private Button _nextButton;
    [SerializeField]
    private float _period;

    private OnCommand _onCommand = new OnCommand();
    public OnCommand OnCommand { get { return _onCommand; } }

    private Coroutine _playProcess;

    public void Initialize(string[] combination)
    {
        _scroller.Initialize(combination);
    }

    public void Play()
    {
        if (_playProcess == null)
        {
            _stopButton.gameObject.SetActive(true);
            _playButton.gameObject.SetActive(false);

            _prevButton.gameObject.SetActive(false);
            _nextButton.gameObject.SetActive(false);

            _playProcess = StartCoroutine(PlayProcess());
        }
    }

    private IEnumerator PlayProcess()
    {
        while (_scroller.Next())
        {
            Next();
            Debug.Log(_scroller.CurrentItem.GetComponentInChildren<Text>().text);
            yield return new WaitForSeconds(_period);
        }

        Stop();
    }

    public void Stop()
    {
        if (_playProcess != null)
        {
            StopCoroutine(_playProcess);
            _playProcess = null;
        }

        _stopButton.gameObject.SetActive(false);
        _playButton.gameObject.SetActive(true);

        _prevButton.gameObject.SetActive(true);
        _nextButton.gameObject.SetActive(true);
    }

    public void Next()
    {
        if (_scroller.Next())
            _onCommand.Invoke(_scroller.CurrentItem.Command);
    }

    public void Prev()
    {
        if (_scroller.Prev())
            _onCommand.Invoke(ReverseCommand(_scroller.CurrentItem.Command));
    }

    private string ReverseCommand(string cmd)
    {
        switch (cmd)
        {
            case "U":
                return "U'";
            case "U'":
                return "U";
            case "R":
                return "R'";
            case "R'":
                return "R";
            case "F":
                return "F'";
            case "F'":
                return "F";
            case "L":
                return "L'";
            case "L'":
                return "L";
            case "B":
                return "B'";
            case "B'":
                return "B";
            case "D":
                return "D'";
            case "D'":
                return "D";
            default:
                return cmd;
        }
    }
}
