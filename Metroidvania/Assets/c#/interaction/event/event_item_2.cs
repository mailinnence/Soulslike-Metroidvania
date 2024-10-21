using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class event_item_2 : MonoBehaviour
{
    public SpriteRenderer objectSprite; // 오브젝트의 SpriteRenderer 참조



    // 텍스트 설명 바꾸기
    public void object_put_down()
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
            if (playerData.event_Item.Contains("간음의 순결") && playerData.Progress == 5 )
            {
   
                playerData.Progress = 6;

                // 변경된 데이터를 다시 JSON 형식으로 변환하여 파일에 저장
                string updatedPlayerJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerPath, updatedPlayerJson);

            }

            // Check if the specified item is in event_Item list
            if (playerData.event_Item.Contains("간음의 순결") && playerData.Progress == 6 )
            {
                // 조건이 맞으면 SpriteRenderer를 보이게 함
                if (objectSprite != null)
                {
                    objectSprite.enabled = true;
                }

                playerData.Progress = 7;

                // 변경된 데이터를 다시 JSON 형식으로 변환하여 파일에 저장
                string updatedPlayerJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerPath, updatedPlayerJson);

            }

                        // Check if the specified item is in event_Item list
            else
            {
                // 조건이 맞으면 SpriteRenderer를 보이게 함
                if (objectSprite != null)
                {
                    objectSprite.enabled = false;
                }
            }
        }
    }



    public void object_put_on()
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
            if (playerData.event_Item.Contains("간음의 순결") && playerData.Progress == 7)
            {
                // 조건이 맞으면 SpriteRenderer를 보이지 않게 함
                if (objectSprite != null)
                {
                    objectSprite.enabled = false;
                }

                // 변경된 데이터를 다시 JSON 형식으로 변환하여 파일에 저장
                string updatedPlayerJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerPath, updatedPlayerJson);
            }
        }
    }



    string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
}
