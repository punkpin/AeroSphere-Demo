using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISnapPoint : MonoBehaviour
{
    public float snapRadius = 1f;
    public bool isOccupied;
    public DraggableUI occupiedUI = null;
    public int snapID;
    public string gameFlagName;

    public bool CanSnap(DraggableUI ui)
    {
        return occupiedUI == null && ui.snapID == snapID;
    }

    public void Snap(DraggableUI ui)
    {
        occupiedUI = ui;
        ui.transform.position = transform.position;
        ui.OnSnapped(this);
        isOccupied = true;
        SetGameFlag(true);
    }

    public void Release()
    {
        occupiedUI = null;
        isOccupied = false;
        SetGameFlag(false);
    }

    public void SetGameFlag(bool value)
    {
        var type = GameManager.instance.GetType();
        var field = type.GetField(gameFlagName);
        if (field != null && field.FieldType == typeof(bool))
        {
            field.SetValue(GameManager.instance, value);
        }
        else
        {
            Debug.LogWarning($"GameManager没有名为{gameFlagName}的bool字段");
        }
    }
}
