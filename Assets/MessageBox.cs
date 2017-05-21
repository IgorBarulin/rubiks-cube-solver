using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MessageBox : UIBehaviour
{
    [SerializeField]
    private Text _messageBoxText;

    [SerializeField]
    private Button _cancelButton;
    [SerializeField]
    private Button _okButton;

    public Button CancelButton { get { return _cancelButton; } }
    public Button OkButton { get { return _okButton; } }

    public void Show(string message, bool showCancelButton = false)
    {
        _messageBoxText.text = message;
        _cancelButton.gameObject.SetActive(showCancelButton);
        gameObject.SetActive(true);
    }
}