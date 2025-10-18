using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftSnapPoint : UISnapPoint
{
    private void OnEnable()
    {
        EventManager.Subscribe(EventType.OnChangeEscCanMove, OnEscCanMoveChanged);
    }

    public override void OnReleaseSnap()
    {
        GameManager.instance.canMoveLeft = false;
    }

    public override void OnStartSnap()
    {
        TryUnlockLeft();
    }

    private void OnEscCanMoveChanged()
    {
        TryUnlockLeft();
    }

    private void TryUnlockLeft()
    {
        if (occupiedUI is Esc esc)
        {
            if (esc.canMove)
            {
                GameManager.instance.canMoveLeft = true;
            }
            else
            {
                GameManager.instance.canMoveLeft = false;
            }
        }
        else
        {
            GameManager.instance.canMoveLeft = false;
        }
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.OnChangeEscCanMove, OnEscCanMoveChanged);
    }
}
