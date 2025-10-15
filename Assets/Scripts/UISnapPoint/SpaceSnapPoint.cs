using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpaceSnapPoint : UISnapPoint
{
    public override void OnReleaseSnap()
    {
        GameManager.instance.canJump = false;
    }

    public override void OnStartSnap()
    {
        GameManager.instance.canJump = true;
    }
}
