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

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        int gridWidth = 20;
        int gridHeight = 20;
        int halfWidth = gridWidth / 2;
        int halfHeight = gridHeight / 2;

        for (int x = -halfWidth; x <= halfWidth; x++)
        {
            Vector3 start = new Vector3(x * GridSize, -halfHeight * GridSize, 0);
            Vector3 end = new Vector3(x * GridSize, halfHeight * GridSize, 0);
            Gizmos.DrawLine(start, end);
        }
        for (int y = -halfHeight; y <= halfHeight; y++)
        {
            Vector3 start = new Vector3(-halfWidth * GridSize, y * GridSize, 0);
            Vector3 end = new Vector3(halfWidth * GridSize, y * GridSize, 0);
            Gizmos.DrawLine(start, end);
        }
    }
}