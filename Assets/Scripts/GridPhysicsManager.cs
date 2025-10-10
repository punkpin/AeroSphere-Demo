using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPhysicsManager : Singleton<GridPhysicsManager>
{
    public int GridSize = 16; // px
    private HashSet<Vector2Int> occupiedCells = new HashSet<Vector2Int>();

    public bool IsCellOccupied(Vector2Int cell)
    {
        return occupiedCells.Contains(cell);
    }

    public void SetCellOccupied(Vector2Int cell, bool occupied)
    {
        if (occupied) occupiedCells.Add(cell);
        else occupiedCells.Remove(cell);
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / GridSize),
            Mathf.FloorToInt(worldPos.y / GridSize)
        );
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(
            gridPos.x * GridSize,
            gridPos.y * GridSize,
            0
        );
    }
}