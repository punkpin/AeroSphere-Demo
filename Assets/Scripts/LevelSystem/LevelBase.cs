using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    public bool hasFirstJump = false;

    protected virtual void Start()
    {
        InitLevel();
    }

    public virtual void InitLevel()
    {
        hasFirstJump = false;
    }

    public virtual void EndLevel()
    {
        Debug.Log("Level Ended");
    }
}
