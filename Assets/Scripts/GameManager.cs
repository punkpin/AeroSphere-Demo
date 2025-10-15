using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool canMoveLeft = true;
    public bool canMoveRight = true;
    public bool canJump = true;
    public LevelBase currentLevel;
}
