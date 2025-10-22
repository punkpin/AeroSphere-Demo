using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvent : MonoBehaviour
{
    public void OnDead()
    {
        GameManager.instance.currentLevel.RestartLevel();
    }
}
