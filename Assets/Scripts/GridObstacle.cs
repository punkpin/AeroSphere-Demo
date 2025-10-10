using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObstacle : MonoBehaviour
{
    public Collider2D col;
    void Start()
    {
        col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogWarning("GridObstacle需要Collider2D组件");
            return;
        }

        Bounds bounds = col.bounds;
        int gridSize = GridPhysicsManager.instance.GridSize;

        Vector2Int min = GridPhysicsManager.instance.WorldToGrid(bounds.min + new Vector3(0.001f, 0.001f, 0));
        Vector2Int max = GridPhysicsManager.instance.WorldToGrid(bounds.max - new Vector3(0.001f, 0.001f, 0));

        for (int x = min.x; x <= max.x; x++)
        {
            for (int y = min.y; y <= max.y; y++)
            {
                GridPhysicsManager.instance.SetCellOccupied(new Vector2Int(x, y), true);
            }
        }
    }
}
