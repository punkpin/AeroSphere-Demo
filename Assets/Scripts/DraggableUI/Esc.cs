using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Esc : DraggableUI
{
    private bool _canMove = false;
    [HideInInspector]public Image image;
    public Sprite esc;
    public Sprite right;

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
        image = GetComponent<Image>();
        _canMove = false;
        image.sprite = esc;
    }
}
