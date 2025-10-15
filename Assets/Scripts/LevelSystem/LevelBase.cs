using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    protected virtual void Start()
    {
        InitLevel();
    }

    public virtual void InitLevel()
    {

    }

    public virtual void EndLevel()
    {
        Debug.Log("Level Ended");
    }
}
