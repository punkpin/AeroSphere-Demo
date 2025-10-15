using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightSnapPoint : UISnapPoint
{
    private void OnEnable()
    {
        EventManager.Subscribe(EventType.OnChangeEscCanMove, OnEscCanMoveChanged);
    }

    public override void OnReleaseSnap()
    {
        GameManager.instance.canMoveRight = false;
    }

    public override void OnStartSnap()
    {
        TryUnlockRight();
    }

    private void OnEscCanMoveChanged()
    {
        TryUnlockRight();
    }

    private void TryUnlockRight()
    {
        if (occupiedUI is Esc esc)
        {
            if (esc.canMove)
            {
                GameManager.instance.canMoveRight = true;
            }
            else
            {
                GameManager.instance.canMoveRight = false;
            }
        }
        else
        {
            GameManager.instance.canMoveRight = false;
        }
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.OnChangeEscCanMove, OnEscCanMoveChanged);
    }
}
