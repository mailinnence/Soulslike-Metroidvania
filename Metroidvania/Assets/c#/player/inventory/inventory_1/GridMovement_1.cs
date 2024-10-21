using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;


public class GridMovement_1 : inventory
{
    [Header("텍스트 바꾸기")]
    public inven_1_text inven_1_text;

    [Header("사운드")]
    public ui_Sound2 ui_Sound2;

    


    private Vector2Int gridPosition;
    private Vector2Int gridSize = new Vector2Int(5, 5);
    private Vector2 cellSize = new Vector2(110f, -120f);
    private Vector2 gridOrigin = new Vector2(110f, 239f); // Position of 1.1 cell

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
        CheckForKey();
        if(Inventory_1_bool)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ui_Sound2._CHANGE_SELECTION_function();
                Move(0, -1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ui_Sound2._CHANGE_SELECTION_function();
                Move(0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ui_Sound2._CHANGE_SELECTION_function();
                Move(-1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ui_Sound2._CHANGE_SELECTION_function();
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



    // 텍스트 설명 바꾸기
   void CheckForKey()
    {
        // Load current_player.json
        string currentPlayerPath = GetSavePath("current_player.json");


        string currentPlayerJson = File.ReadAllText(currentPlayerPath);
        CurrentPlayerData currentPlayerData = JsonUtility.FromJson<CurrentPlayerData>(currentPlayerJson);
        int currentPlayer = currentPlayerData.current_player;

        // Load player{n}.json based on current_player
        string playerPath = GetSavePath($"player{currentPlayer}.json");
        if (File.Exists(playerPath))
        {
            string playerJson = File.ReadAllText(playerPath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(playerJson);

            // 오브젝트의 위치로 설명 텍스트 판단
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

            // Check if the specified item is in event_Item list
            if (playerData.event_Item.Contains("잊혀진 열쇠") && gridPosition.x == 0 && gridPosition.y == 0 )
            {
                // 조건이 맞으면 UI 이미지를 보이게 함
                // Debug.Log($"Player {currentPlayer} has '잊혀진 열쇠' in event_Item.");
                inven_1_text.ChangeText_1();
            }
            else if (playerData.event_Item.Contains("간음의 순결") && gridPosition.x == 1 && gridPosition.y == 0 )
            {
                inven_1_text.ChangeText_2();
            }
            else if (playerData.event_Item.Contains("피로 그린 장미") && gridPosition.x == 2 && gridPosition.y == 0 )
            {
                inven_1_text.ChangeText_3();
            }
            else
            {
                inven_1_text.ChangeText_else();
            }
        }
    }

    string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

}
