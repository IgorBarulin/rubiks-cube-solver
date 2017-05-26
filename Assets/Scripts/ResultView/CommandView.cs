using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandView : UIBehaviour
{
    [SerializeField]
    private GameObject _backlight;

    public bool Backlight
    {
        get { return _backlight.activeSelf; }
        set { _backlight.SetActive(value); }
    }

    public string Command { get; private set; }

    public void Initialize(string command)
    {
        Command = command;
    }

    protected override void Start()
    {
        Backlight = false;
    }
}
