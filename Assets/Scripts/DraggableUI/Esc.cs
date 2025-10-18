using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class Esc : DraggableUI
{
    private bool _canMove = false;
    public TMP_Text escText;

    public bool canMove
    {
        get => _canMove;
        set
        {
            if (_canMove != value)
            {
                _canMove = value;
                EventManager.Trigger(EventType.OnChangeEscCanMove);
            }
        }
    }

    private void Start()
    {
        _canMove = false;
        escText.text = "Esc";
    }
}
