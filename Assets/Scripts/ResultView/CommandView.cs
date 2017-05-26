using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandView : UIBehaviour
{
    [SerializeField]
    private GameObject _backlight;

    private Text _text;

    public bool Backlight
    {
        get { return _backlight.activeSelf; }
        set { _backlight.SetActive(value); }
    }

    public string Command { get; private set; }

    public void Initialize(string command)
    {
        Command = command;
        _text.text = Command;
    }

    protected override void Awake()
    {
        base.Awake();

        _text = gameObject.GetComponentInChildren<Text>();
    }

    protected override void Start()
    {
        base.Start();

        Backlight = false;
    }
}
