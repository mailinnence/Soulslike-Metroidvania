using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;


public class GridMovement_2 : inventory
{
    [Header("텍스트 바꾸기")]
    public inven_2_text inven_2_text;

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
        if(Inventory_2_bool)
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




            // 튜토리얼 지역 ----------------------------------------------------------------------------------------------------
            if (playerData.bible.Contains("테레아서(3:1)") && gridPosition.x == 0 && gridPosition.y == 0 )
            {
                inven_2_text.ChangeText_1();
            }

            else if (playerData.bible.Contains("테레아서(3:6)") && gridPosition.x == 1 && gridPosition.y == 0 )
            {
                inven_2_text.ChangeText_2();
            }

            else if (playerData.bible.Contains("테레아서(3:11)") && gridPosition.x == 2 && gridPosition.y == 0 )
            {
                inven_2_text.ChangeText_3();
            }

            else if (playerData.bible.Contains("테레아서(3:17)") && gridPosition.x == 3 && gridPosition.y == 0 )
            {
                inven_2_text.ChangeText_4();
            }

            else if (playerData.bible.Contains("테레아서(3:22)") && gridPosition.x == 4 && gridPosition.y == 0 )
            {
                inven_2_text.ChangeText_5();
            }  



            // page_2 ------------------------------------------------------------------------------------------------
            else if (playerData.bible.Contains("메이사가서(7:14)") && gridPosition.x == 0 && gridPosition.y == 1 )
            {
                inven_2_text.ChangeText_6();
            }

            else if (playerData.bible.Contains("메이사가서(8:22)") && gridPosition.x == 1 && gridPosition.y == 1 )
            {
                inven_2_text.ChangeText_7();
            }


            else if (playerData.bible.Contains("메이사가서(15:3)") && gridPosition.x == 2 && gridPosition.y == 1 )
            {
                inven_2_text.ChangeText_8();
            }


            else if (playerData.bible.Contains("메이사가서(20:1)") && gridPosition.x == 3 && gridPosition.y == 1 )
            {
                inven_2_text.ChangeText_9();
            }


            else if (playerData.bible.Contains("메이사가서(25:6)") && gridPosition.x == 4 && gridPosition.y == 1 )
            {
                inven_2_text.ChangeText_10();
            }


            else
            {
                inven_2_text.ChangeText_else();
            }
        }
    }



    string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

}
