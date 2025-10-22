using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : LevelBase
{
    public List<GameObject> disappearedUI = new List<GameObject>();
    public Enemy enemy;

    public Rigidbody2D hp;
    public Rigidbody2D esc;
    public Rigidbody2D set;

    private Vector3 hpInitPos;
    private Vector3 escInitPos;
    private Vector3 setInitPos;

    private void Start()
    {
        InitLevel();
    }
    public override void InitLevel()
    {
        base.InitLevel();
        foreach (GameObject gameObject in disappearedUI)
        {
            gameObject.SetActive(false);
        }
        GameManager.instance.canJump = false;
        GameManager.instance.canMoveLeft = false;
        GameManager.instance.canMoveRight = false;
        GameManager.instance.currentLevel.hasFirstJump = true;
        hpInitPos = hp.transform.position;
        escInitPos = esc.transform.position;
        setInitPos = set.transform.position;
    }

    public override void Restart()
    {
        base.Restart();
        enemy.boxCollider.enabled = true;
        enemy.animator.Play("Idle");
        hp.gravityScale = 0;
        esc.gravityScale = 0;
        set.gravityScale = 0;
        hp.transform.position = hpInitPos;
        esc.transform.position = escInitPos;
        set.transform.position = setInitPos;
    }
}
