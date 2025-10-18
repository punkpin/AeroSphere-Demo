using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2:LevelBase
{
    public List<GameObject> disappearedUI = new List<GameObject>();
    public override void InitLevel()
    {
        foreach(GameObject gameObject in disappearedUI)
        {
            gameObject.SetActive(false);
        }
    } 



}
