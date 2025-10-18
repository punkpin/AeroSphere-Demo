using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UISnapPoint : MonoBehaviour
{
    public float snapRadius = 1f;
    public bool isOccupied;
    public DraggableUI occupiedUI = null;
    public string snapID;

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
        OnStartSnap();
    }

    public void Release()
    {
        occupiedUI = null;
        isOccupied = false;
        OnReleaseSnap();
    }

    public abstract void OnStartSnap();

    public abstract void OnReleaseSnap();
}
