using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class coordinate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 
    // 다음 씬 정보 수정
    public void save_coordinate(float x , float y)
    {
        string path = Application.persistentDataPath + "/current_player.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            CurrentPlayerData currentPlayerData = JsonUtility.FromJson<CurrentPlayerData>(json);
            int currentPlayer = currentPlayerData.current_player;

            string playerPath = Application.persistentDataPath + $"/player{currentPlayer}.json";
            if (File.Exists(playerPath))
            {
                string playerJson = File.ReadAllText(playerPath);
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(playerJson);
                
                // 좌표 초기화 ---------------------------------------------
                if (playerData.save_coordinate == null)
                {
                    playerData.save_coordinate = new List<float>();
                }
                else
                {
                    playerData.save_coordinate.Clear();
                }


                // Add the new coordinates
                playerData.save_coordinate.Add(x);
                playerData.save_coordinate.Add(y);

                // Save the updated player data back to the file
                string updatedJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerPath, updatedJson);
            }
        }
    }

}
