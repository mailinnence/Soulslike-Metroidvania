using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class KeyChecker_1 : MonoBehaviour
{
    public string itemName; // 확인할 아이템 이름
    public Image targetImage; // 조건에 따라 보이거나 보이지 않게 할 UI 이미지

    void Start()
    {
        CheckForKey(); // Start 시점에 한번 체크
    }

    void Update()
    {
        CheckForKey(); // 매 프레임마다 체크
    }

    void CheckForKey()
    {
        // Load current_player.json
        string currentPlayerPath = GetSavePath("current_player.json");
        // if (!File.Exists(currentPlayerPath))
        // {
        //     Debug.LogError("current_player.json not found.");
        //     return;
        // }

        string currentPlayerJson = File.ReadAllText(currentPlayerPath);
        CurrentPlayerData currentPlayerData = JsonUtility.FromJson<CurrentPlayerData>(currentPlayerJson);
        int currentPlayer = currentPlayerData.current_player;

        // Load player{n}.json based on current_player
        string playerPath = GetSavePath($"player{currentPlayer}.json");
        if (File.Exists(playerPath))
        {
            string playerJson = File.ReadAllText(playerPath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(playerJson);

            // Check if the specified item is in event_Item list
            if (playerData.event_Item.Contains(itemName))
            {
                // 조건이 맞으면 UI 이미지를 보이게 함
                targetImage.enabled = true;
                // Debug.Log($"Player {currentPlayer} has '{itemName}' in event_Item.");
            }
            else
            {
                // 조건이 맞지 않으면 UI 이미지를 보이지 않게 함
                targetImage.enabled = false;
                // Debug.Log($"Player {currentPlayer} does not have '{itemName}' in event_Item.");
            }
        }
        else
        {
            // player{n}.json 파일이 없을 때도 UI 이미지를 보이지 않게 함
            targetImage.enabled = false;
        }
    }

    string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
}