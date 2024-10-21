using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement_5 : inventory
{
    private Vector2Int gridPosition;
    private Vector2Int gridSize = new Vector2Int(5, 2);
    private Vector2 cellSize = new Vector2(110f, -120f);
    private Vector2 gridOrigin = new Vector2(110f, -121f); // Position of 1.1 cell

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Initialize starting position (1,1) corresponds to (0,0) in gridPosition
        gridPosition = new Vector2Int(0, 0);
        UpdatePosition();
    }

    void Update()
    {
        if(Inventory_5_bool)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(0, -1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(-1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(1, 0);
            }
        }
    }

    void Move(int x, int y)
    {
        Vector2Int newPosition = gridPosition + new Vector2Int(x, y);

        // Check if the new position is within the bounds of the grid
        if (newPosition.x >= 0 && newPosition.x < gridSize.x && newPosition.y >= 0 && newPosition.y < gridSize.y)
        {
            gridPosition = newPosition;
            UpdatePosition();
        }
    }

    void UpdatePosition()
    {
        Vector2 newPosition = gridOrigin + new Vector2(gridPosition.x * cellSize.x, gridPosition.y * cellSize.y);
        rectTransform.anchoredPosition = newPosition;
    }
}
