using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : LevelBase
{
    public List<Rigidbody2D> oldUI = new List<Rigidbody2D>();
    public List<Rigidbody2D> newUI = new List<Rigidbody2D>();
    public List<UISnapPoint> uISnapPoints = new List<UISnapPoint>();

    private void Start()
    {
        InitLevel();
    }

    private void OnEnable()
    {
        EventManager.Subscribe(EventType.OnLevel1FirstJump, OnFirstJump);
    }

    public override void InitLevel()
    {
        base.InitLevel();
        foreach (Rigidbody2D rb in oldUI)
        {
            rb.gravityScale = 0f;
        }
        foreach (Rigidbody2D rb in newUI)
        {
            rb.gravityScale = 0f;
        }
        foreach (UISnapPoint point in uISnapPoints)
        {
            point.isOccupied = true;
        }
        GameManager.instance.canMoveLeft = true;
        GameManager.instance.canMoveRight = true;
    }

    public void OnFirstJump()
    {
        if (!hasFirstJump)
        {
            hasFirstJump = true;
            foreach (Rigidbody2D rb in oldUI)
            {
                rb.gravityScale = 1f;
            }
            foreach (Rigidbody2D rb in newUI)
            {
                rb.gravityScale = 1f;
            }
            foreach (UISnapPoint point in uISnapPoints)
            {
                point.isOccupied = false;
                point.OnReleaseSnap();
            }
        }
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.OnLevel1FirstJump, OnFirstJump);
    }
}
