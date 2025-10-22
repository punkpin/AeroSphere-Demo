using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    public bool hasFirstJump = false;
    private Vector3 initPosition;

    public virtual void InitLevel()
    {
        hasFirstJump = false;
        GameManager.instance.player.HP = 3;
        initPosition = GameManager.instance.player.transform.position;
    }

    public virtual void EndLevel()
    {
        Debug.Log("Level Ended");
        GameManager.instance.LoadNextScene(GameManager.instance.OnSceneLoaded);
    }

    public virtual void RestartLevel()
    {
        GameManager.instance.player.transform.position = initPosition;
        GameManager.instance.player.animator.Play("Idle");
        Restart();
        InitLevel();
    }

    public virtual void Restart()
    {

    }
}
